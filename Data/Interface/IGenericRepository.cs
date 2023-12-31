using Cross.Cutting.Helper;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interface
{
    public interface IGenericRepository<T> : IRepository
    {
        Task<IList<T>> GetAll(
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            string includedProperties = "",
            int? howMany = null);

        Task<IEnumerable<TResult>> GetAllSelectedColumns<TResult>(
            Func<T, TResult> selectColumns,
            IEnumerable<Expression<Func<T, bool>>> filters,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includedProperties = "",
            int? howMany = null);

        Task<Pagination<T>> GetAllPaged(int page, int pageSize,
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            string includedProperties = "");

        Task<int> EntityCount(Expression<Func<T, bool>> filter = null);

        Task<IDbContextTransaction> GetTransaction();

        Task<T> Get(Expression<Func<T, bool>> filter, string includedProperties = "");

        Task<TResult> GetSelected<TResult>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TResult>> selectColumns,
            string includedProperties = "");

        Task UpdateOnQuery(
            List<Expression<Func<T, bool>>> filters,
            Expression<Func<T, T>> updateColumns,
            string includedProperties = null);

        Task<bool> EntityExist(Expression<Func<T, bool>> filter, string includedProperties = null);

        Task Save(T entity);
        Task SaveTransaction(T entity);

        Task Delete(T entity);
        Task DeleteTransaction(T entity);
    }
}
