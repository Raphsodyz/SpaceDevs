using System.Linq.Expressions;
using Business.Interface;
using Cross.Cutting.Helper;
using Data.Interface;
using Domain.Entities;

namespace Business.Business
{
    public abstract class BusinessViewBase<T, TRepository> : IBusinessViewBase<T, TRepository>
        where T : class
        where TRepository : IGenericViewRepository<T>
    {
        public IUnitOfWork _uow;
        protected TRepository _repository;

        public BusinessViewBase(IUnitOfWork uow)
        {
            _uow = uow;
            _repository = (TRepository)uow.Repository(typeof(TRepository));
        }

        public async Task<IList<T>> GetAll(IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, int? howMany = null)
        {
            return await _repository.GetAll(filters, orderBy, howMany);
        }

        public async Task<IEnumerable<TObject>> GetAllSelectedColumns<TResult, TObject>(Expression<Func<T, TResult>> selectColumns, IEnumerable<Expression<Func<T, bool>>> filters, Func<TResult, TObject> buildObject, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? howMany = null)
        {
            return await _repository.GetAllSelectedColumns(selectColumns, filters, buildObject, orderBy, howMany);
        }

        public async Task<T> GetById(Expression<Func<T, bool>> filter)
        {
            return await _repository.GetById(filter);
        }

        public async Task<TObject> GetSelected<TResult, TObject>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selectColumns, Func<TResult, TObject> buildObject)
        {
            return await _repository.GetSelected(filter, selectColumns, buildObject);
        }

        public async Task<Pagination<T>> GetViewPaged(int page, int pageSize, IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null)
        {
            return await _repository.GetViewPaged(page, pageSize, filters, orderBy);
        }

        public async Task<int> EntityCount(Expression<Func<T, bool>> filter = null)
        {
            return await _repository.EntityCount(filter);
        }
    }
}