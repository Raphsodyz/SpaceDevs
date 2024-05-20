using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interface
{
    public interface IPadRepository : IGenericRepository<Pad>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Pad, TResult>> selectColumns, string includedProperties = null);    }
    }
