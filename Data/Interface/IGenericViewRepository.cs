using System.Linq.Expressions;
using Cross.Cutting.Helper;

namespace Data.Interface
{
    public interface IGenericViewRepository<T> : IRepository
    {
        Task<T> GetById(Expression<Func<T, bool>> filter);

        Task<IList<T>> GetAll(
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            int? howMany = null);
            
        Task<Pagination<T>> GetViewPaged(int page, int pageSize,
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null);

        Task<TResult> GetSelected<TResult>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TResult>> selectColumns);

        Task<IEnumerable<TResult>> GetAllSelectedColumns<TResult>(
            Func<T, TResult> selectColumns,
            IEnumerable<Expression<Func<T, bool>>> filters,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? howMany = null);

        Task<int> EntityCount(Expression<Func<T, bool>> filter = null);
    }
}