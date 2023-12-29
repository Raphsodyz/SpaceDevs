using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Services.Request
{
    [SwaggerSchema(Description = "Request Query")]
    public class SearchLaunchRequest
    {
        [Display(Name = "Mission")]
        [DataType(DataType.Text)]
        [StringLength(360, ErrorMessage = "Attention! The character length in the field {0} is invalid.", MinimumLength = 2)]
        public string? Mission { get; set; }

        [Display(Name = "Rocket")]
        [DataType(DataType.Text)]
        [StringLength(360, ErrorMessage = "Attention! The character length in the field {0} is invalid.", MinimumLength = 2)]
        public string? Rocket { get; set; }

        [Display(Name = "Location")]
        [DataType(DataType.Text)]
        [StringLength(360, ErrorMessage = "Attention! The character length in the field {0} is invalid.", MinimumLength = 2)]
        public string? Location { get; set; }

        [Display(Name = "Pad")]
        [DataType(DataType.Text)]
        [StringLength(360, ErrorMessage = "Attention! The character length in the field {0} is invalid.", MinimumLength = 2)]
        public string? Pad { get; set; }

        [Display(Name = "Launch")]
        [DataType(DataType.Text)]
        [StringLength(360, ErrorMessage = "Attention! The character length in the field {0} is invalid.", MinimumLength = 2)]
        public string? Launch { get; set; }
    }
}