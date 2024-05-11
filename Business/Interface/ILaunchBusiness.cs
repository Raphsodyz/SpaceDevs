using Data.Interface;
using Domain.Entities;
using System.Linq.Expressions;

namespace Business.Interface
{
    public interface ILaunchBusiness : IBusinessBase<Launch, ILaunchRepository>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Launch, TResult>> selectColumns, string includedProperties = null);
        Task SaveOnUpdateLaunch(Launch launch, Guid? idStatus, Guid? idLaunchServiceProvider, Guid? idRocket, Guid? idMission, Guid? idPad);
    }
}
