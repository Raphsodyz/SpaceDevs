using Domain.Shared;

namespace Domain.Commands.Launch.Responses
{
    public class SoftDeleteLaunchResponse : BaseCommandResponse
    {
        public SoftDeleteLaunchResponse(bool success, string error)
        {
            Success = success;
            Error = error;
        }
    }
}