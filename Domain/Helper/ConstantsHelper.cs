using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helper
{
    public class ConstantsHelper
    {
        
    }

    public static class ErrorMessages
    {
        public static readonly string InternalServerError = "Attention! The Service is unavailable.";
        public static readonly string NullArgument = "Attention! Submit all the data necessary to complete the request.";
        public static readonly string KeyNotFound = "Attention! The requested data was not found.";
        public static readonly string NoData = "Attention! There is no data in the database.";
        public static readonly string InvalidPageSelected = "Attention! The selected page does not exist.";
        public static readonly string LaunchApiEndPointError = "Attention! The SpaceDevs API endpoint returned an error.";
        public static readonly string DeserializingEndPointContentError = "Attention! An error ocurred when retrieving a JSON data from Space Devs API.";
        public static readonly string NoDataFromSpaceDevApi = "Attention! There is no data received from Space Devs Api. Check the service disponibility and try again.";
    }

    public static class SuccessMessages
    {
        public static readonly string DeletedEntity = "The selected entity has deleted with success.";
    }

    public static class EndPoints
    {
        public static readonly string TheSpaceDevsLaunchEndPoint = "TheSpaceDevsLaunchEndPoint";
    }
}
