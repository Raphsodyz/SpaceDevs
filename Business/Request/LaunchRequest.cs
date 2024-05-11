using System.ComponentModel.DataAnnotations;
using Cross.Cutting.Helper;

namespace Business.Request
{
    public class LaunchRequest
    {
        [Display(Name = "ID Launch")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0}: " + ErrorMessages.NullArgument)]
        public Guid? launchId { get; set; }

        public LaunchRequest()
        {
            
        }

        public LaunchRequest(Guid? launchId)
        {
            this.launchId = launchId;
        }
    }
}