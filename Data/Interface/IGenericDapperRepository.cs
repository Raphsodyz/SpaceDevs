using System.Data.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Interface
{
    public interface IGenericDapperRepository : IRepository
    {
        Task<TResult> GetSelected<TResult>(string query, object parameters = null, DbConnection sharedConnection = null, IDbContextTransaction transaction = null);
        Task<IEnumerable<TResult>> GetAllSelected<TResult>(string query, object parameters = null, DbConnection sharedConnection = null, IDbContextTransaction transaction = null);
        Task Save<T>(T entity, DbConnection sharedConnection = null, IDbContextTransaction transaction = null) where T : BaseEntity;
        Task FullUpdate<T>(T entity, string where, DbConnection sharedConnection = null, IDbContextTransaction transaction = null) where T : BaseEntity;
        Task ExecuteSql(string query, object parameters = null, DbConnection sharedConnection = null, IDbContextTransaction transaction = null);
    }
}