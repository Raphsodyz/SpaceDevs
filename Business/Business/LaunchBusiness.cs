using Business.Interface;
using Data.Interface;
using Domain.Entities;
using System.Linq.Expressions;

namespace Business.Business
{
    public class LaunchBusiness : BusinessBase<Launch, ILaunchRepository>, ILaunchBusiness, IBusiness
    {
        public LaunchBusiness(IUnitOfWork uow):base(uow)
        {
            
        }

        public async Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Launch, TResult>> selectColumns, string includedProperties = null)
        {
            return await _repository.ILikeSearch(searchTerm, selectColumns, includedProperties);
        }

        public async Task SaveOnUpdateLaunch(Launch launch, Guid? idStatus, Guid? idLaunchServiceProvider, Guid? idRocket, Guid? idMission, Guid? idPad)
        {
            launch.IdStatus = idStatus;
            launch.IdLaunchServiceProvider = idLaunchServiceProvider;
            launch.IdRocket = idRocket;
            launch.IdMission = idMission;
            launch.IdPad = idPad;

            //Setup null navigation properties for ef change tracker..
            launch.Status = null;
            launch.LaunchServiceProvider = null;
            launch.Rocket = null;
            launch.Mission = null;
            launch.Pad = null;
    
            await _repository.Save(launch);
        }
    }
}
