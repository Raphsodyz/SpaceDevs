using System.ComponentModel.DataAnnotations;

namespace Services.ViewModel
{
    public class MissionViewModel
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "The field {0} can't be null.")]
        public Guid Id { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "LaunchLibrary")]
        public int? LaunchLibraryId { get; set; }

        [Display(Name = "Name")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Name.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Description.", MinimumLength = 2)]
        public string Description { get; set; }

        [Display(Name = "Type")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Type.", MinimumLength = 2)]
        public string Type { get; set; }

        public OrbitViewModel Orbit { get; set; }

    }
}