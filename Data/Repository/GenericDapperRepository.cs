using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Dapper;
using Data.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Data.Repository
{
    public class GenericDapperRepository<T> : IGenericDapperRepository<T> where T : BaseEntity
    {
        private readonly string _connectionString;
        private const int maxEntityReturn = 10;
        private readonly string table = typeof(T)?.GetCustomAttribute<TableAttribute>()?.Name;
        public GenericDapperRepository()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = Environment.GetEnvironmentVariable(configuration.GetSection("ConnectionStrings:default").Value);
        }

        public async Task<TResult> GetSelected<TResult>(string columns, string where, object parameters, IDbContextTransaction transaction = null)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            try
            {
                connection.Open();

                string query = $"SELECT {columns} FROM {table} WHERE {where}";
                var result = await connection.QueryFirstOrDefaultAsync<TResult>(query, parameters, transaction?.GetDbTransaction());

                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }
    
        public async Task<IEnumerable<TResult>> GetAllSelected<TResult>(string columns, object parameters, int? howMany = null, string where = null, IDbContextTransaction transaction = null)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            try
            {
                connection.Open();
                string query = $"SELECT LIMIT {howMany ?? maxEntityReturn} {columns} FROM {table}";
                
                if(!string.IsNullOrWhiteSpace(where))
                    query += " WHERE " + where;
                
                var result = await connection.QueryAsync<TResult>(query, parameters, transaction?.GetDbTransaction());
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }
    
        public async Task<T> Get(string where, object parameters, IDbContextTransaction transaction = null)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            try
            {
                connection.Open();

                string query = $"SELECT * FROM {table} WHERE {where}";
                var result = await connection.QueryFirstOrDefaultAsync<T>(query, parameters, transaction?.GetDbTransaction());

                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }
    
        public async Task<IEnumerable<T>> GetAll(object parameters, int? howMany = null, string where = null, IDbContextTransaction transaction = null)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            try
            {
                connection.Open();

                string query = $"SELECT LIMIT {howMany ?? maxEntityReturn} * FROM {table}";
                
                if(!string.IsNullOrWhiteSpace(where))
                    query += " WHERE " + where;
                
                var result = await connection.QueryAsync<T>(query, parameters, transaction?.GetDbTransaction());
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }
    
        public async Task Save(T entity, IDbContextTransaction transaction = null)
        {
            //Remeber to generate the Guid ID first in the server!!
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                connection.Open();

                entity.Id = Guid.NewGuid();
                entity.ImportedT = DateTime.Now;
                entity.AtualizationDate = DateTime.Now;
                entity.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

                string query = $"INSERT INTO {table}({GetColumnNames()}) VALUES ({GetPropNames()})";
                await connection.ExecuteAsync(query, entity, transaction?.GetDbTransaction());
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        public async Task Update(string where, string set, object parameters, IDbContextTransaction transaction = null)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                connection.Open();

                string query = $"UPDATE {table} SET {set} WHERE {where}";
                await connection.ExecuteAsync(query, parameters, transaction?.GetDbTransaction());
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        public async Task Delete(string where, object parameters, IDbContextTransaction transaction = null)
        {
            var connection = new NpgsqlConnection(_connectionString);
            try
            {
                connection.Open();

                string query = $"DELETE FROM {table} WHERE {where}";
                await connection.ExecuteAsync(query, parameters, transaction?.GetDbTransaction());
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        private string GetPropNames()
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

        private string GetColumnNames()
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
    }
}