using Business.Interface;
using Domain.Entities;
using Domain.Interface;
using System.Linq.Expressions;

namespace Application.Business
{
    public class LocationBusiness : BusinessBase<Location, ILocationRepository>, ILocationBusiness, IBusiness
    {
        public LocationBusiness(IUnitOfWork uow):base(uow)
        {
            
        }

        public async Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Location, TResult>> selectColumns, string includedProperties = null)
        {
            return await _repository.ILikeSearch(searchTerm, selectColumns, includedProperties);
        }
    }
}
