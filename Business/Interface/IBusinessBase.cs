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
        Task Delete(Guid id);
        Task<int> EntityCount(Expression<Func<T, bool>> filter = null);
        Task<T> Get(Expression<Func<T, bool>> filter, string includedProperties = "");
        Task<IList<T>> GetAll(IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "", int? howMany = null);
        Task<Pagination<T>> GetAllPaged(int page, int pageSize, IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "");
        Task<IEnumerable<TObject>> GetAllSelectedColumns<TResult, TObject>(Expression<Func<T, TResult>> selectColumns, IEnumerable<Expression<Func<T, bool>>> filters, Func<TResult, TObject> buildObject, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includedProperties = "", int? howMany = null);
        Task<TObject> GetSelected<TResult, TObject>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selectColumns, Func<TResult, TObject> buildObject, string includedProperties = "");
        Task UpdateOnQuery(List<Expression<Func<T, bool>>> filters, Expression<Func<T, T>> updateColumns);
        Task UpdateOnQuery(Expression<Func<T, bool>> filter, Expression<Func<T, T>> updateColumns);
        Task AddToChangeTracker(T entity);
        Task Save(T entity);
        Task<bool> EntityExist(Expression<Func<T, bool>> filter);
    }
}
