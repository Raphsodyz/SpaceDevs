using Domain.Materializated.Views;
using Domain.Shared;

namespace Domain.Queries.Launch.Responses
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