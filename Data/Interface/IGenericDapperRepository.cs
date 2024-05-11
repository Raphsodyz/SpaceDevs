using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Interface
{
    public interface IGenericDapperRepository<T> : IRepository
    {
        Task<TResult> GetSelected<TResult>(string columns, string where, object parameters, DbConnection sharedConnection = null, IDbContextTransaction transaction = null);
        Task<IEnumerable<TResult>> GetAllSelected<TResult>(string columns, object parameters, int? howMany = null, string where = null, DbConnection sharedConnection = null, IDbContextTransaction transaction = null);
        Task<T> Get(string where, object parameters, DbConnection sharedConnection = null, IDbContextTransaction transaction = null);
        Task<IEnumerable<T>> GetAll(object parameters, int? howMany = null, string where = null, DbConnection sharedConnection = null, IDbContextTransaction transaction = null);
        Task Save(T entity, DbConnection sharedConnection = null, IDbContextTransaction transaction = null);
        Task PartialUpdate(string where, string set, object parameters, DbConnection sharedConnection = null, IDbContextTransaction transaction = null);
        Task FullUpdate(T entity, string where, DbConnection sharedConnection = null, IDbContextTransaction transaction = null);
        Task Delete(string where, object parameters, DbConnection sharedConnection = null, IDbContextTransaction transaction = null);
    }
}