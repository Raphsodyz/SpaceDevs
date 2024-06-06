namespace Infrastructure.Persistence.Context.Factory
{
    public interface IDbContextFactory
    {
        BaseContext GetContext(string contextName);
    }
}