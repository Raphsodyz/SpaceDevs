using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ViewModel
{
    public class SearchLaunchViewModel
    {
        [Display(Name = "Mission")]
        [DataType(DataType.Text)]
        [StringLength(360, ErrorMessage = "Attention! The length of the field {0} is invalid.", MinimumLength = 2)]
        public string? Mission { get; set; }

        [Display(Name = "Rocket")]
        [DataType(DataType.Text)]
        [StringLength(360, ErrorMessage = "Attention! The length of the field {0} is invalid.", MinimumLength = 2)]
        public string? Rocket { get; set; }

        [Display(Name = "Location")]
        [DataType(DataType.Text)]
        [StringLength(360, ErrorMessage = "Attention! The length of the field {0} is invalid.", MinimumLength = 2)]
        public string? Location { get; set; }

        [Display(Name = "Launch")]
        [DataType(DataType.Text)]
        [StringLength(360, ErrorMessage = "Attention! The length of the field {0} is invalid.", MinimumLength = 2)]
        public string? Launch { get; set; }
    }
}