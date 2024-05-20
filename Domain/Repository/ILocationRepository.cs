using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Interface
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Location, TResult>> selectColumns, string includedProperties = null);    }
    }