using Domain.Entities;
using Domain.Interface;
using System.Linq.Expressions;

namespace Application.Interface
{
    public interface IConfigurationBusiness : IBusinessBase<Configuration, IConfigurationRepository>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Configuration, TResult>> selectColumns, string includedProperties = null);
    }
}
