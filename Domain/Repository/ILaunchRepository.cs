using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interface
{
    public interface ILaunchRepository : IGenericRepository<Launch>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Launch, TResult>> selectColumns, string includedProperties = null);
        Task SetUpBaseEntityDTO(Launch launch);
        Task SaveOnUpdateLaunch(Launch launch, Guid? idStatus, Guid? idLaunchServiceProvider, Guid? idRocket, Guid? idMission, Guid? idPad);
    }
}
