namespace Infrastructure.Persistence.Context.Factory
{
    public class DbContextFactory
    {
        private readonly IDictionary<string, BaseContext> _contexts;

        public DbContextFactory(IDictionary<string, BaseContext> contexts)
        {
            _contexts = contexts;
        }

        public BaseContext GetContext(string contextName)
        {
            return _contexts[contextName];
        }
    }
}