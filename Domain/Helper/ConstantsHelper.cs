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
        public static readonly string InternalServerError = "Atention! The Service is unavailable.";
        public static readonly string NullArgument = "Atention! Submit all the data necessary to complete the request.";
        public static readonly string KeyNotFound = "Atention! The requested data was not found.";
        public static readonly string NoData = "Atention! There is no data in the database.";
        public static readonly string InvalidPageSelected = "Atention! The selected page does not exist.";
    }

    public static class SuccessMessages
    {
        public static readonly string DeletedEntity = "The selected entity has deleted with success.";
    }
}
