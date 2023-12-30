using Data.Interface;
using Data.Repository;

namespace Data.Context
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

        public void Save()
        {
            _context.SaveChanges();
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
