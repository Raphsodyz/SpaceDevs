using Core.Domain.Entities;
using System.Linq.Expressions;

namespace Core.Database.Repository
{
    public interface IConfigurationRepository : IGenericRepository<Configuration>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Configuration, TResult>> selectColumns, string includedProperties = null);    }
    }
