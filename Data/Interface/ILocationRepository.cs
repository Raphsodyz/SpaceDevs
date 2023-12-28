using Domain.Entities;

namespace Data.Interface
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Func<Location, TResult> selectColumns, string includedProperties = null);    }
    }