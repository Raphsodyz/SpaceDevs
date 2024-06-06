using Domain.Materializated.Views;
using Domain.Shared;

namespace Domain.Commands.Launch.Responses
{
    public class UpdateOneLaunchResponse : BaseCommandResponse
    {
        public UpdateOneLaunchResponse(bool success, string message)
        {
            Success = success;
            Error = message;
        }
    }
}