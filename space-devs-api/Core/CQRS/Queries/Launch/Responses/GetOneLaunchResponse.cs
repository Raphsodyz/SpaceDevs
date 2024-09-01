using Core.Materializated.Views;
using Core.Shared;

namespace Core.CQRS.Queries.Launch.Responses
{
    public class GetOneLaunchResponse : BaseQueryResponse<LaunchView>
    {
        public GetOneLaunchResponse(bool success, string error, LaunchView data)
        {
            Success = success;
            Error = error;
            Data = data;
        }
    }
}