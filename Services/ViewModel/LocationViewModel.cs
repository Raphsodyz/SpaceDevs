using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ViewModel
{
    public class LocationViewModel
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "The field {0} can't be null.")]
        public Guid Id { get; set; }

        [Display(Name = "Link")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid link.", MinimumLength = 2)]
        public string Url { get; set; }

        [Display(Name = "Slug")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Slug.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Country Code")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Country Code.", MinimumLength = 2)]
        public string CountryCode { get; set; }

        [Display(Name = "Map Image")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Map Image link.", MinimumLength = 2)]
        [DataType(DataType.ImageUrl)]
        public string MapImage { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Total Launch Count")]
        public int TotalLaunchCount { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Total Landing Count")]
        public int TotalLandingCount { get; set; }

    }
}