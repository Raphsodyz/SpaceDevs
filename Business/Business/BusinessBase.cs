using Business.Interface;
using Data.Interface;
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
            return type.Name switch
            {
                "IConfigurationBusiness" => new ConfigurationBusiness(_uow),
                "ILaunchBusiness" => new LaunchBusiness(_uow),
                "ILaunchServiceProviderBusiness" => new LaunchServiceProviderBusiness(_uow),
                "ILocationBusiness" => new LocationBusiness(_uow),
                "IMissionBusiness" => new MissionBusiness(_uow),
                "IOrbitBusiness" => new OrbitBusiness(_uow),
                "IPadBusiness" => new PadBusiness(_uow),
                "IRocketBusiness" => new RocketBusiness(_uow),
                "IStatusBusiness" => new StatusBusiness(_uow),
                "IUpdateLogBusiness" => new UpdateLogBusiness(_uow),
                "ILaunchViewBusiness" => new LaunchViewBusiness(_uow),
                _ => throw new NotImplementedException()
            };
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

        public async Task<IList<T>> GetAll(IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "", int? howMany = null)
        {
            return await _repository.GetAll(filters, orderBy, includedProperties, howMany);
        }

        public async Task<Pagination<T>> GetAllPaged(int page, int pageSize, IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "")
        {
            return await _repository.GetAllPaged(page, pageSize, filters, orderBy, includedProperties);
        }

        public async Task<IEnumerable<TObject>> GetAllSelectedColumns<TResult, TObject>(Expression<Func<T, TResult>> selectColumns, IEnumerable<Expression<Func<T, bool>>> filters, Func<TResult, TObject> buildObject, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includedProperties = "", int? howMany = null)
        {
            return await _repository.GetAllSelectedColumns(selectColumns, filters, buildObject, orderBy, includedProperties, howMany);
        }

        public async Task<TObject> GetSelected<TResult, TObject>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selectColumns, Func<TResult, TObject> buildObject, string includedProperties = "")
        {
            return await _repository.GetSelected(filter, selectColumns, buildObject, includedProperties);
        }

        public async Task UpdateOnQuery(List<Expression<Func<T, bool>>> filters, Expression<Func<T, T>> updateColumns)
        {
            await _repository.UpdateOnQuery(filters, updateColumns);
        }

        public async Task UpdateOnQuery(Expression<Func<T, bool>> filter, Expression<Func<T, T>> updateColumns)
        {
            await _repository.UpdateOnQuery(filter, updateColumns);
        }

        public async Task Save(T entity)
        {
            await _repository.Save(entity);
        }

        public async Task SaveTransaction(T entity)
        {
            await _repository.SaveTransaction(entity);
        }

        public async Task<bool> EntityExist(Expression<Func<T, bool>> filter)
        {
            return await _repository.EntityExist(filter);
        }
    }
}
