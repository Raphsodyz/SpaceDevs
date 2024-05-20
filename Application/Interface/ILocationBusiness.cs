using Application.Interface;
using Domain.Entities;
using Domain.Interface;
using System.Linq.Expressions;

namespace Business.Interface
{
    public interface ILocationBusiness : IBusinessBase<Location, ILocationRepository>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Location, TResult>> selectColumns, string includedProperties = null);
    }
}
