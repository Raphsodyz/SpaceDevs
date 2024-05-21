using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Materializated.Views;

namespace Domain.Queries.Launch.Responses
{
    public class GetOneLaunchResponse : BaseResponse<LaunchView>
    {
        public GetOneLaunchResponse(bool success, string error, LaunchView data)
        {
            Success = success;
            Error = error;
            Data = data;
        }
    }
}