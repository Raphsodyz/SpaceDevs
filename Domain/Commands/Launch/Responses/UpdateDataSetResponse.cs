using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shared;

namespace Domain.Commands.Launch.Responses
{
    public class UpdateDataSetResponse : BaseResponse<object?>
    {
        public UpdateDataSetResponse(bool success, string message)
        {
            Success = success;
            Error = message;
            Data = null;
        }       
    }
}