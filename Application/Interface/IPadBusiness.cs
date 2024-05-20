using Application.Interface;
using Domain.Entities;
using Domain.Interface;
using System.Linq.Expressions;

namespace Business.Interface
{
    public interface IPadBusiness : IBusinessBase<Pad, IPadRepository>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Pad, TResult>> selectColumns, string includedProperties = null);
    }
}
