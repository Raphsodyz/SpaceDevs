using System.ComponentModel.DataAnnotations;
using Cross.Cutting.Helper;

namespace Business.DTO.Request
{
    public class LaunchRequest
    {
        [Display(Name = "ID Launch")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0}: " + ErrorMessages.NullArgument)]
        public Guid? LaunchId { get; set; }

        public LaunchRequest()
        {
            
        }

        public LaunchRequest(Guid? launchId)
        {
            LaunchId = launchId;
        }
    }
}