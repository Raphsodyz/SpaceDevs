using Business.Interface;
using Domain.Entities;
using Domain.Interface;
using System.Linq.Expressions;

namespace Application.Business
{
    public class PadBusiness : BusinessBase<Pad, IPadRepository>, IPadBusiness, IBusiness
    {
        public PadBusiness(IUnitOfWork uow):base(uow)
        {
            
        }

        public async Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Pad, TResult>> selectColumns, string includedProperties = null)
        {
            return await _repository.ILikeSearch(searchTerm, selectColumns, includedProperties);
        }
    }
}
