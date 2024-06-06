using Cross.Cutting.Helper;
using Domain.Materializated.Views;
using Domain.Shared;

namespace Domain.Queries.Launch.Responses
{
    public class SeachByParamResponse : BaseQueryResponse<Pagination<LaunchView>>
    {
        public SeachByParamResponse(bool success, string error, Pagination<LaunchView> data)
        {
            Success = success;
            Error = error;
            Data = data;
        }
    }
}