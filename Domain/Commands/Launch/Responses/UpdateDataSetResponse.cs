using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shared;

namespace Domain.Commands.Launch.Responses
{
    public class UpdateDataSetResponse : BaseResponse<bool?>
    {
        public UpdateDataSetResponse(bool success, string message, bool? updated)
        {
            Success = success;
            Error = message;
            Data = updated;
        }       
    }
}