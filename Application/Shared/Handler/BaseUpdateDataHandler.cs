using Cross.Cutting.Helper;
using Domain.Entities;
using Domain.Interface;

namespace Application.Shared.Handler
{
    public abstract class BaseUpdateDataHandler
    {
        private readonly IUnitOfWork _uow;
        protected BaseUpdateDataHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        protected async Task SaveLaunch(Launch launch, bool replaceData)
        {
            if (ObjectHelper.IsObjectEmpty(launch))
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            ILaunchRepository _launchRepository = _uow.Repository(typeof(ILaunchRepository)) as ILaunchRepository;
            if(replaceData == false)
            {
                if(await _launchRepository.EntityExist(l => l.ApiGuid == launch.ApiGuid))
                    return;
            }
            else
                await SetOriginalBaseEntityDataProcesses(launch);
        }
    }
}