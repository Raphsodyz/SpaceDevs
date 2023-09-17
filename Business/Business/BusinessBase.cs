using Business.Interface;
using Data.Interface;
using Data.Repository;
using Domain.Entities;
using Domain.Helper;
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

        public virtual void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        public virtual void DeleteTransaction(T entity)
        {
            _repository.DeleteTransaction(entity);
        }

        public virtual int EntityCount(Expression<Func<T, bool>> filter = null)
        {
            return _repository.EntityCount(filter);
        }

        public virtual T Get(Expression<Func<T, bool>> filter, string includedProperties = "")
        {
            return _repository.Get(filter, includedProperties);
        }

        public virtual IList<T> GetAll(IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "")
        {
            return _repository.GetAll(filters, orderBy, includedProperties);
        }

        public virtual Pagination<T> GetAllPaged(int page, int pageSize, IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "")
        {
            return _repository.GetAllPaged(page, pageSize, filters, orderBy, includedProperties);
        }

        public virtual IEnumerable<TResult> GetAllSelectedColumns<TResult>(IEnumerable<Expression<Func<T, bool>>> filters = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includedProperties = "", int? howMany = null, Func<T, TResult> selectColumns = null)
        {
            return _repository.GetAllSelectedColumns(filters, orderBy, includedProperties, howMany, selectColumns);
        }

        public virtual IList<T> GetMany(IEnumerable<Expression<Func<T, bool>>> filters = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includedProperties = "", int? howMany = null)
        {
            return _repository.GetMany(filters, orderBy, includedProperties, howMany);
        }

        public virtual TResult GetSelected<TResult>(Expression<Func<T, bool>> filter = null, string includedProperties = "", Func<T, TResult> selectColumns = null)
        {
            return _repository.GetSelected(filter, includedProperties, selectColumns);
        }

        public void Save(T entity)
        {
            _repository.Save(entity);
        }

        public void SaveTransaction(T entity)
        {
            _repository.SaveTransaction(entity);
        }
    }
}
