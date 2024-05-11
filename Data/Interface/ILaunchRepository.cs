using Domain.Entities;
using System.Linq.Expressions;

namespace Data.Interface
{
    public interface ILaunchRepository : IGenericRepository<Launch>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Launch, TResult>> selectColumns, string includedProperties = null);
    }
}
