using Core.Domain.Entities;
using System.Linq.Expressions;

namespace Core.Database.Repository
{
    public interface IPadRepository : IGenericRepository<Pad>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Pad, TResult>> selectColumns, string includedProperties = null);    }
    }
