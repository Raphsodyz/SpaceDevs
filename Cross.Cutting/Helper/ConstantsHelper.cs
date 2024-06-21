using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross.Cutting.Helper
{
    public class ConstantsHelper
    {
        
    }

    public static class ErrorMessages
    {
        public const string InternalServerError = "Attention! The Service is unavailable.";
        public const string NullArgument = "Attention! Submit all the data necessary to complete the request.";
        public const string KeyNotFound = "Attention! The requested data was not found.";
        public const string NoData = "Attention! There is no data in the database.";
        public const string InvalidPageSelected = "Attention! The selected page does not exist.";
        public const string LaunchApiEndPointError = "Attention! The SpaceDevs API endpoint returned an error.";
        public const string DeserializingContentError = "Attention! An error ocurred when retrieving a JSON data from Space Devs API. Contatc the sys admin to get support.";
        public const string NoDataFromSpaceDevApi = "Attention! There is no data received from Space Devs Api. Check the service disponibility and try again.";
        public const string UpdateJobError = "Attention! The update job has failed.";
        public const string StoredProcedurePublishedRoutineError = "Attention! The update to published stored procedure has failed.";
        public const string ViewNotExists = "Attention! The launch view not exists. Contact the sys admin to get support.";
        public const string ForeignKeyNotFound = "Attention! The selected foreign key does not exists.";
    }

    public static class SuccessMessages
    {
        public const string DeletedEntity = "The selected entity has deleted with success.";
        public const string PartialImportSuccess = "The Data offset has loaded with success.";
        public const string ImportedDataSuccess = "The Data loading from Space Devs Api was successfull.";
        public const string UpdateJob = "The Data loading from Space Devs Api by the update job was successfull.";
    }

    public static class EndPoints
    {
        public const string TheSpaceDevsLaunchEndPoint = "https://ll.thespacedevs.com/2.2.0/launch/";
    }

    public static class LaunchNestedObjectsForeignKeys
    {
        public const string ROCKET = "IdConfiguration";
        public const string MISSION = "IdOrbit";
        public const string PAD = "IdLocation";
    }

    public static class ContextNames
    {
        public const string FutureSpaceCommand = "FutureSpaceCommand";
        public const string FutureSpaceQuery = "FutureSpaceQuery";
    }

    public static class RedisCollectionsKeys
    {
        public const string SingleLaunchKey = "Launch:";
        public const string PaginatatedLaunchKey = "PaginatedLaunchPage:";
        public const string SearchLaunchKey = "PaginatedLaunchSearchKey:";
        public const string SeachLaunchPage = "PaginatedLaunchSearchPage:";
    }

    public static class RedisDefaultMinutesTTL
    {
        public const int LargeRedisTTL = 60;
        public const int LowRedisTTL = 5;
    }

    public static class ConnectionStrings
    {
        public const string Postgresql = "ConnectionStrings:POSTGRESQL_CONNECTION_STRING";
        public const string Redis = "ConnectionStrings:REDIS_CONNECTION_STRING";

    }
}
