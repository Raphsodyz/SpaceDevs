using Cross.Cutting.Helper;
using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Repository;

namespace Infrastructure.Persistence.Context
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public FutureSpaceContext _context;
        private readonly Dictionary<Type, IRepository> _repository;
        public UnitOfWork(FutureSpaceContext context)
        {
            _context = context;
            _repository = new Dictionary<Type, IRepository>();
        }

        public IGenericDapperRepository Dapper()
        {
            return new GenericDapperRepository();
        }

        public void SetupForeignKey<T>(T entity, string foreignKeyName, Guid desiredFkValue) where T : BaseEntity
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            var foreignKeys = entityType.GetProperties()
                .FirstOrDefault(p => p.Name.Equals(foreignKeyName, StringComparison.OrdinalIgnoreCase));

            _ = foreignKeys ?? throw new ArgumentException(ErrorMessages.ForeignKeyNotFound);

            foreignKeys.PropertyInfo.SetValue(entity, desiredFkValue);
        }

        public IRepository Repository(Type type)
        {
            if (!_repository.ContainsKey(type))
            {
                IRepository repository;
                switch (type.Name)
                {
                    case "IConfigurationRepository":
                        repository = new ConfigurationRepository(_context);
                        break;
                    case "ILaunchRepository":
                        repository = new LaunchRepository(_context);
                        break;
                    case "ILaunchServiceProviderRepository":
                        repository = new LaunchServiceProviderRepository(_context);
                        break;
                    case "ILocationRepository":
                        repository = new LocationRepository(_context);
                        break;
                    case "IMissionRepository":
                        repository = new MissionRepository(_context);
                        break;
                    case "IOrbitRepository":
                        repository = new OrbitRepository(_context);
                        break;
                    case "IPadRepository":
                        repository = new PadRepository(_context);
                        break;
                    case "IRocketRepository":
                        repository = new RocketRepository(_context);
                        break;
                    case "IStatusRepository":
                        repository = new StatusRepository(_context);
                        break;
                    case "IUpdateLogRepository":
                        repository = new UpdateLogRepository(_context);
                        break;
                    case "ILaunchViewRepository":
                        repository = new LaunchViewRepository(_context);
                        break;
                    default:
                        return null;
                }
                _repository.Add(type, repository);
            }
            return _repository[type];
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
