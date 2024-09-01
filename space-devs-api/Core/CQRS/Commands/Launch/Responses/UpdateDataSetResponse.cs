using Core.Shared;

namespace Core.CQRS.Commands.Launch.Responses
{
    public class UpdateDataSetResponse : BaseCommandResponse
    {
        public UpdateDataSetResponse(bool success, string message)
        {
            Success = success;
            Error = message;
        }       
    }
}