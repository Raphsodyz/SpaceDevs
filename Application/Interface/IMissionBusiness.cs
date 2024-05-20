using Application.Interface;
using Domain.Entities;
using Domain.Interface;
using System.Linq.Expressions;

namespace Business.Interface
{
    public interface IMissionBusiness : IBusinessBase<Mission, IMissionRepository>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Mission, TResult>> selectColumns, string includedProperties = null);
    }
}
