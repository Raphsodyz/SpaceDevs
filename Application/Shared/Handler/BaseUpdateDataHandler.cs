using Cross.Cutting.Helper;
using Domain.Entities;
using Domain.Interface;

namespace Application.Shared.Handler
{
    public abstract class BaseUpdateDataHandler
    {
        private readonly ILaunchRepository _launchRepository;
        protected BaseUpdateDataHandler(ILaunchRepository launchRepository)
        {
            _launchRepository = launchRepository;
        }

        protected async Task SaveLaunch(Launch launch, bool replaceData)
        {
            if (ObjectHelper.IsObjectEmpty(launch))
                throw new ArgumentNullException(ErrorMessages.NullArgument);

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