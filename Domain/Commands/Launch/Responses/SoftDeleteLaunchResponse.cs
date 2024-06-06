using Domain.Shared;

namespace Domain.Commands.Launch.Responses
{
    public class SoftDeleteLaunchResponse : BaseResponse<object?>
    {
        public SoftDeleteLaunchResponse(bool success, string error)
        {
            Success = success;
            Error = error;
            Data = null;
        }
    }
}