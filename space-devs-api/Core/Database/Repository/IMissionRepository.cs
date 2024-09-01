using Core.Domain.Entities;
using System.Linq.Expressions;

namespace Core.Database.Repository
{
    public interface IMissionRepository : IGenericRepository<Mission>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Mission, TResult>> selectColumns, string includedProperties = null);    }
    }
