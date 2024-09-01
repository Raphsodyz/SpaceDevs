using Core.Shared;

namespace Core.CQRS.Commands.Launch.Responses
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