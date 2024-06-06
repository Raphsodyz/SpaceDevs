using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Dapper;
using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Repository.Helper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Repository
{
    public class GenericDapperRepository : IGenericDapperRepository
    {
        private readonly string _queryConnectionString;
        private readonly string _commandConnectionString;
        public GenericDapperRepository()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _queryConnectionString = Environment.GetEnvironmentVariable(configuration.GetSection("ConnectionStrings:Query").Value);
            _commandConnectionString = Environment.GetEnvironmentVariable(configuration.GetSection("ConnectionStrings:Command").Value);
        }

        public async Task<TResult> GetSelected<TResult>(string query, object parameters = null, DbConnection sharedConnection = null, DbTransaction transaction = null)
        {
            return await DapperConnectionHelper.ResolveConnection(async (connection) =>
            {
                var result = await connection.QueryFirstOrDefaultAsync<TResult>(query, parameters, transaction);
                return result;
            }, sharedConnection, _queryConnectionString);
        }
    
        public async Task<IEnumerable<TResult>> GetAllSelected<TResult>(string query, object parameters = null, DbConnection sharedConnection = null, DbTransaction transaction = null)
        {
            return await DapperConnectionHelper.ResolveConnection(async (connection) =>
            {
                var result = await connection.QueryAsync<TResult>(query, parameters, transaction);
                return result;
            }, sharedConnection, _queryConnectionString);
        }
    
        public async Task Save<T>(T entity, DbConnection sharedConnection = null, DbTransaction transaction = null) where T : BaseEntity
        {
            //Remeber to generate the BaseEntity props first in the server for new entities!!
            await DapperConnectionHelper.ResolveConnection(async (connection) =>
            {
                string query = $"INSERT INTO {typeof(T)?.GetCustomAttribute<TableAttribute>()?.Name}({GetColumnNames<T>()}) VALUES ({GetPropNames<T>()})";
                await connection.ExecuteAsync(query, entity, transaction);
            }, sharedConnection, _commandConnectionString);
        }

        public async Task FullUpdate<T>(T entity, string where, DbConnection sharedConnection = null, DbTransaction transaction = null) where T : BaseEntity
        {
            await DapperConnectionHelper.ResolveConnection(async (connection) =>
            {
                string query = $"UPDATE {typeof(T)?.GetCustomAttribute<TableAttribute>()?.Name} SET {FullUpdateSetString(GetColumnNames<T>(), GetPropNames<T>())} WHERE {where}";
                await connection.ExecuteAsync(query, entity, transaction);
            }, sharedConnection, _commandConnectionString);
        }
        
        public async Task ExecuteSql(string query, object parameters = null, DbConnection sharedConnection = null, DbTransaction transaction = null)
        {
            await DapperConnectionHelper.ResolveConnection(async (connection) =>
            {
                await connection.ExecuteAsync(query, parameters, transaction);
            }, sharedConnection, _commandConnectionString);
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
            {
                if(propSplitted[i].Trim().Equals("@Id", StringComparison.OrdinalIgnoreCase))
                    continue;

                builder.Append(columnsSplitted[i].Trim() + " = " + propSplitted[i].Trim() + ',');
            }
            
            builder.Length--;
            return builder.ToString();
        }
    }
}