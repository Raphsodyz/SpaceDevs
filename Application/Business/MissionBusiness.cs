using Business.Interface;
using Domain.Entities;
using Domain.Interface;
using System.Linq.Expressions;

namespace Application.Business
{
    public class MissionBusiness : BusinessBase<Mission, IMissionRepository>, IMissionBusiness, IBusiness
    {
        public MissionBusiness(IUnitOfWork uow):base(uow)
        {
            
        }

        public async Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Mission, TResult>> selectColumns, string includedProperties = null)
        {
            return await _repository.ILikeSearch(searchTerm, selectColumns, includedProperties);
        }
    }
}
