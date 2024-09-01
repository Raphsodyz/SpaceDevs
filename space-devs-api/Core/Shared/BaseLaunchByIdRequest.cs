using System.ComponentModel.DataAnnotations;
using Cross.Cutting.Helper;

namespace Core.Shared
{
    public abstract class BaseLaunchByIdRequest
    {
        [Display(Name = "ID Launch")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0}: " + ErrorMessages.NullArgument)]
        public Guid? launchId { get; set; }
    }
}