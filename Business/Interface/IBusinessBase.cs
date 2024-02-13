using Data.Interface;
using Domain.Entities;
using Cross.Cutting.Helper;
using System.Linq.Expressions;

namespace Business.Interface
{
    public interface IBusinessBase<T, TRepository>
        where T : BaseEntity
        where TRepository : IGenericRepository<T>
    {
        Task Delete(T entity);
        Task DeleteTransaction(T entity);
        Task<int> EntityCount(Expression<Func<T, bool>> filter = null);
        Task<T> Get(Expression<Func<T, bool>> filter, string includedProperties = "");
        Task<IList<T>> GetAll(IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "", int? howMany = null);
        Task<Pagination<T>> GetAllPaged(int page, int pageSize, IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "");
        Task<IEnumerable<TResult>> GetAllSelectedColumns<TResult>(Expression<Func<T, TResult>> selectColumns, IEnumerable<Expression<Func<T, bool>>> filters, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includedProperties = "", int? howMany = null);
        Task<TResult> GetSelected<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selectColumns, string includedProperties = "");
        Task UpdateOnQuery(List<Expression<Func<T, bool>>> filters, Expression<Func<T, T>> updateColumns, string includedProperties = null);
        Task Save(T entity);
        Task SaveTransaction(T entity);
        Task<bool> EntityExist(Expression<Func<T, bool>> filter, string includedProperties = null);
    }
}
