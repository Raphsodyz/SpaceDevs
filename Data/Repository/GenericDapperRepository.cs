using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Dapper;
using Data.Interface;
using Data.Repository.Helper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Data.Repository
{
    public class GenericDapperRepository : IGenericDapperRepository
    {
        private readonly string _connectionString;
        public GenericDapperRepository()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = Environment.GetEnvironmentVariable(configuration.GetSection("ConnectionStrings:default").Value);
        }

        public async Task<TResult> GetSelected<TResult>(string query, object parameters = null, DbConnection sharedConnection = null, IDbContextTransaction transaction = null)
        {
            return await DapperConnectionHelper.ResolveConnection(async (connection) =>
            {
                var trans = transaction?.GetDbTransaction();
                var result = await connection.QueryFirstOrDefaultAsync<TResult>(query, parameters, trans);

                return result;
            }, sharedConnection, _connectionString);
        }
    
        public async Task<IEnumerable<TResult>> GetAllSelected<TResult>(string query, object parameters = null, DbConnection sharedConnection = null, IDbContextTransaction transaction = null)
        {
            return await DapperConnectionHelper.ResolveConnection(async (connection) =>
            {
                var trans = transaction?.GetDbTransaction();
                var result = await connection.QueryAsync<TResult>(query, parameters, trans);
                
                return result;
            }, sharedConnection, _connectionString);
        }
    
        public async Task Save<T>(T entity, DbConnection sharedConnection = null, IDbContextTransaction transaction = null) where T : BaseEntity
        {
            //Remeber to generate the BaseEntity props first in the server for new entities!!
            await DapperConnectionHelper.ResolveConnection(async (connection) =>
            {
                var trans = transaction?.GetDbTransaction();
                string query = $"INSERT INTO {typeof(T)?.GetCustomAttribute<TableAttribute>()?.Name}({GetColumnNames<T>()}) VALUES ({GetPropNames<T>()})";

                await connection.ExecuteAsync(query, entity, trans);
            }, sharedConnection, _connectionString);
        }

        public async Task FullUpdate<T>(T entity, string where, DbConnection sharedConnection = null, IDbContextTransaction transaction = null) where T : BaseEntity
        {
            await DapperConnectionHelper.ResolveConnection(async (connection) =>
            {
                var trans = transaction?.GetDbTransaction();
                string query = $"UPDATE {typeof(T)?.GetCustomAttribute<TableAttribute>()?.Name} SET {FullUpdateSetString(GetColumnNames<T>(), GetPropNames<T>())} WHERE {where}";

                await connection.ExecuteAsync(query, entity, trans);
            }, sharedConnection, _connectionString);
        }
        
        public async Task ExecuteSql(string query, object parameters = null, DbConnection sharedConnection = null, IDbContextTransaction transaction = null)
        {
            await DapperConnectionHelper.ResolveConnection(async (connection) =>
            {
                var trans = transaction?.GetDbTransaction();
                await connection.ExecuteAsync(query, parameters, trans);
            }, sharedConnection, _connectionString);
        }

        private string GetPropNames<T>() where T : BaseEntity
        {
            string[] undesiredProps = new string[]{"@Search", "@IsNew"};
            var properties = typeof(T).GetProperties().Select(prop => 
            {
                if (prop.PropertyType.IsClass && prop.PropertyType.Namespace == typeof(T).Namespace)
                    return null;

                string name = string.IsNullOrWhiteSpace(prop.Name) ? null : $"@{prop.Name}";

                if(undesiredProps.Contains(name))
                    return null;

                return name;
            })
            .Where(name => name != null);

            return string.Join(", ", properties).Trim();
        }

        private string GetColumnNames<T>() where T : BaseEntity
        {
            var properties = typeof(T).GetProperties();
            var columns = properties.Select(prop =>
            {
                string column = prop?.GetCustomAttribute<ColumnAttribute>()?.Name;

                if(column == "search")
                    return null;

                return string.IsNullOrWhiteSpace(column) ? null : column;
            })
            .Where(column => column != null);

            return string.Join(", ", columns).Trim();
        }
    
        private string FullUpdateSetString(string columnNames, string propNames)
        {
            string[] columnsSplitted = columnNames.Split(',');
            string[] propSplitted = propNames.Split(',');

            StringBuilder builder = new();

            for(int i = 0; i < propSplitted.Length; i++)
                builder.Append(columnsSplitted[i].Trim() + " = " + propSplitted[i].Trim() + ',');
            
            builder.Length--;
            return builder.ToString();
        }
    }
}