using System.Data;
using System.Data.Common;
using Npgsql;

namespace Infrastructure.Persistence.Repository.Helper
{
    public static class DapperConnectionHelper
    {
        public static async Task ResolveConnection(
            Func<DbConnection, Task> action,
            DbConnection sharedConnection = null,
            string connectionString = null)
        {
            bool shouldDisposeConnection = sharedConnection == null;
            DbConnection connection = sharedConnection ?? new NpgsqlConnection(connectionString);

            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                await action(connection);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (shouldDisposeConnection)
                    await connection.DisposeAsync();
            }
        }

        public static async Task<TResult> ResolveConnection<TResult>(
            Func<DbConnection, Task<TResult>> action,
            DbConnection sharedConnection = null,
            string connectionString = null)
        {
            bool shouldDisposeConnection = sharedConnection == null;
            DbConnection connection = sharedConnection ?? new NpgsqlConnection(connectionString);

            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                return await action(connection);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (shouldDisposeConnection)
                    await connection.DisposeAsync();
            }
        }
    }
}