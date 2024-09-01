using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Database.Repository
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Location, TResult>> selectColumns, string includedProperties = null);    }
    }