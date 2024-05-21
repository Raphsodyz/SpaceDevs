using System.ComponentModel.DataAnnotations;
using Cross.Cutting.Helper;
using Domain.Queries.Launch.Responses;

namespace Domain.Request
{
    public class LaunchByIdRequest
    {
        [Display(Name = "ID Launch")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0}: " + ErrorMessages.NullArgument)]
        public Guid? launchId { get; set; }

        public LaunchByIdRequest()
        {
            
        }

        public LaunchByIdRequest(Guid? launchId)
        {
            this.launchId = launchId;
        }
    }
}