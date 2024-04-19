using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Interface
{
    public interface IGenericDapperRepository<T> : IRepository
    {
        Task<TResult> GetSelected<TResult>(string columns, string where, object parameters, IDbContextTransaction transaction = null);
        Task<IEnumerable<TResult>> GetAllSelected<TResult>(string columns, object parameters, int? howMany = null, string where = null, IDbContextTransaction transaction = null);
        Task<T> Get(string where, object parameters, IDbContextTransaction transaction = null);
        Task<IEnumerable<T>> GetAll(object parameters, int? howMany = null, string where = null, IDbContextTransaction transaction = null);
        Task Save(T entity, IDbContextTransaction transaction = null);
        Task Update(string where, string set, object parameters, IDbContextTransaction transaction = null);
        Task Delete(string where, object parameters, IDbContextTransaction transaction = null);
    }
}