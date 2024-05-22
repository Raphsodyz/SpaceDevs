using Domain.Materializated.Views;
using Domain.Shared;

namespace Domain.Commands.Launch.Responses
{
    public class UpdateOneLaunchResponse : BaseResponse<LaunchView>
    {
        public UpdateOneLaunchResponse(bool success, string message, LaunchView launchView)
        {
            Success = success;
            Error = message;
            Data = launchView;
        }
    }
}