using Core.Domain.Entities;
using System.Linq.Expressions;

namespace Core.Database.Repository
{
    public interface ILaunchRepository : IGenericRepository<Launch>
    {
        Task SoftDeleteLaunchById(Guid launchId);
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Launch, TResult>> selectColumns, string includedProperties = null);
    }
}
