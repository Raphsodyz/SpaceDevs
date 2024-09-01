using System.Linq.Expressions;
using Cross.Cutting.Helper;

namespace Core.Database.Repository
{
    public interface IGenericViewRepository<T>
    {
        Task<T> GetById(Expression<Func<T, bool>> filter);

        Task<IList<T>> GetAll(
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            int? howMany = null);
            
        Task<Pagination<T>> GetViewPaged(int page, int pageSize,
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null);

        Task<TObject> GetSelected<TResult, TObject>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TResult>> selectColumns,
            Func<TResult, TObject> buildObject);

        Task<IEnumerable<TObject>> GetAllSelectedColumns<TResult, TObject>(
            Expression<Func<T, TResult>> selectColumns,
            IEnumerable<Expression<Func<T, bool>>> filters,
            Func<TResult, TObject> buildObject,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? howMany = null);

        Task<int> EntityCount(Expression<Func<T, bool>> filter = null);
    }
}