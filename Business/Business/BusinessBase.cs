using Business.Interface;
using Data.Interface;
using Data.Repository;
using Domain.Entities;
using Cross.Cutting.Helper;
using System.Linq.Expressions;

namespace Business.Business
{
    public abstract class BusinessBase<T, TRepository> : IBusinessBase<T, TRepository>
        where T : BaseEntity
        where TRepository : IGenericRepository<T>
    {
        public IUnitOfWork _uow;
        protected TRepository _repository;
        public BusinessBase(IUnitOfWork uow)
        {
            _uow = uow;
            _repository = (TRepository)uow.Repository(typeof(TRepository));
        }

        public IBusiness GetBusiness(Type type)
        {
            switch (type.Name)
            {
                case "IConfigurationBusiness":
                    return new ConfigurationBusiness(_uow);
                case "ILaunchBusiness":
                    return new LaunchBusiness(_uow);
                case "ILaunchServiceProviderBusiness":
                    return new LaunchServiceProviderBusiness(_uow);
                case "ILocationBusiness":
                    return new LocationBusiness(_uow);
                case "IMissionBusiness":
                    return new MissionBusiness(_uow);
                case "IOrbitBusiness":
                    return new OrbitBusiness(_uow);
                case "IPadBusiness":
                    return new PadBusiness(_uow);
                case "IRocketBusiness":
                    return new RocketBusiness(_uow);
                case "IStatusBusiness":
                    return new StatusBusiness(_uow);
                case "IUpdateLogBusiness":
                    return new UpdateLogBusiness(_uow);
                default:
                    throw new NotImplementedException();
            }
        }

        public async Task Delete(T entity)
        {
            await _repository.Delete(entity);
        }

        public async Task DeleteTransaction(T entity)
        {
            await _repository.DeleteTransaction(entity);
        }

        public async Task<int> EntityCount(Expression<Func<T, bool>> filter = null)
        {
            return await _repository.EntityCount(filter);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter, string includedProperties = "")
        {
            return await _repository.Get(filter, includedProperties);
        }

        public async Task<IList<T>> GetAll(IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "")
        {
            return await _repository.GetAll(filters, orderBy, includedProperties);
        }

        public async Task<Pagination<T>> GetAllPaged(int page, int pageSize, IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "")
        {
            return await _repository.GetAllPaged(page, pageSize, filters, orderBy, includedProperties);
        }

        public async Task<IEnumerable<TResult>> GetAllSelectedColumns<TResult>(Func<T, TResult> selectColumns, IEnumerable<Expression<Func<T, bool>>> filters, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includedProperties = "", int? howMany = null)
        {
            return await _repository.GetAllSelectedColumns(selectColumns, filters, orderBy, includedProperties, howMany);
        }

        public async Task<IList<T>> GetMany(IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "", int? howMany = null)
        {
            return await _repository.GetMany(filters, orderBy, includedProperties, howMany);
        }

        public async Task<TResult> GetSelected<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selectColumns, string includedProperties = "")
        {
            return await _repository.GetSelected(filter, selectColumns, includedProperties);
        }

        public async Task UpdateOnQuery(List<Expression<Func<T, bool>>> filters, Expression<Func<T, T>> updateColumns, string includedProperties = null)
        {
            await _repository.UpdateOnQuery(filters, updateColumns, includedProperties);
        }

        public async Task Save(T entity)
        {
            await _repository.Save(entity);
        }

        public async Task SaveTransaction(T entity)
        {
            await _repository.SaveTransaction(entity);
        }
    }
}
