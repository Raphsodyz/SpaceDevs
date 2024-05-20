using Cross.Cutting.Helper;
using System.Data.Common;
using System.Linq.Expressions;

namespace Domain.Interface
{
    public interface IGenericRepository<T> : IRepository
    {
        Task<IList<T>> GetAll(
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            string includedProperties = "",
            int? howMany = null);

        Task<IEnumerable<TObject>> GetAllSelectedColumns<TResult, TObject>(
            Expression<Func<T, TResult>> selectColumns,
            IEnumerable<Expression<Func<T, bool>>> filters,
            Func<TResult, TObject> buildObject,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includedProperties = "",
            int? howMany = null);

        Task<Pagination<T>> GetAllPaged(int page, int pageSize,
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            string includedProperties = "");

        Task<int> EntityCount(Expression<Func<T, bool>> filter = null);

        Task BeginTransaction();

        Task CommitTransaction();

        Task RollbackTransaction();

        DbTransaction GetCurrentlyTransaction();

        DbConnection GetEfConnection();

        Task<T> Get(Expression<Func<T, bool>> filter, string includedProperties = "");

        Task<TObject> GetSelected<TResult, TObject>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TResult>> selectColumns,
            Func<TResult, TObject> buildObject,
            string includedProperties = "");

        Task UpdateOnQuery(
            List<Expression<Func<T, bool>>> filters,
            Expression<Func<T, T>> updateColumns);

        Task UpdateOnQuery(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, T>> updateColumns);

        Task<bool> EntityExist(Expression<Func<T, bool>> filter);

        Task AddToChangeTracker(T entity);

        Task Save(T entity);

        Task Delete(T entity);

        Task Delete(Guid id);
    }
}
