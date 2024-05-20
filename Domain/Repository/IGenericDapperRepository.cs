using System.Data;
using System.Data.Common;
using Domain.Entities;

namespace Domain.Interface
{
    public interface IGenericDapperRepository : IRepository
    {
        Task<TResult> GetSelected<TResult>(string query, object parameters = null, DbConnection sharedConnection = null, DbTransaction transaction = null);
        Task<IEnumerable<TResult>> GetAllSelected<TResult>(string query, object parameters = null, DbConnection sharedConnection = null, DbTransaction transaction = null);
        Task Save<T>(T entity, DbConnection sharedConnection = null, DbTransaction transaction = null) where T : BaseEntity;
        Task FullUpdate<T>(T entity, string where, DbConnection sharedConnection = null, DbTransaction transaction = null) where T : BaseEntity;
        Task ExecuteSql(string query, object parameters = null, DbConnection sharedConnection = null, DbTransaction transaction = null);
    }
}