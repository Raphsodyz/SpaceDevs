using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ViewModel
{
    public class PadViewModel
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "The field {0} can't be null.")]
        public Guid Id { get; set; }

        [Display(Name = "Link")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid link.", MinimumLength = 2)]
        public string Url { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Agency")]
        public int? AgencyId { get; set; }

        [Display(Name = "Name")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Name.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Info URL")]
        public string InfoUrl { get; set; }

        [Display(Name = "Wiki URL")]
        public string WikiUrl { get; set; }

        [Display(Name = "Map URL")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Map URL.", MinimumLength = 2)]
        public string MapUrl { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Longitude")]
        public double Longitude { get; set; }

        public LocationViewModel Location { get; set; }

        [Display(Name = "Map Image")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Map Image link.", MinimumLength = 2)]
        [DataType(DataType.ImageUrl)]
        public string MapImage { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Total Launch Count")]
        public int TotaLaunchCount { get; set; }
    }
}