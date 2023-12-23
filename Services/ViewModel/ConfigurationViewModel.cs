using System.ComponentModel.DataAnnotations;

namespace Services.ViewModel
{
    public class ConfigurationViewModel
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "The field {0} can't be null.")]
        public Guid Id { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Launch Library")]
        public int? LaunchLibraryId { get; set; }

        [Display(Name = "Link")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid link.", MinimumLength = 2)]
        public string Url { get; set; }

        [Display(Name = "Name")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Name.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Family")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Family.", MinimumLength = 2)]
        public string Family { get; set; }

        [Display(Name = "Full Name")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Full Name.", MinimumLength = 2)]
        public string FullName { get; set; }

        [Display(Name = "Variant")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Variant.", MinimumLength = 2)]
        public string Variant { get; set; }
    }
}