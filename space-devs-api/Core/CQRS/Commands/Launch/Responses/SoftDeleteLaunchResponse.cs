using Core.Shared;

namespace Core.CQRS.Commands.Launch.Responses
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