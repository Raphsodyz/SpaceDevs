using System.ComponentModel.DataAnnotations;

namespace Business.Request
{
    public class UpdateLaunchRequest
    {
        [Range(0, 100, ErrorMessage = "The value on the field {0} must be greater than 0 and less 100.")]
        [Display(Name = "Limit")]
        public int? Limit { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [Display(Name = "Skip")]
        public int? Skip { get; set; }

        [Range(0, 15, ErrorMessage = "The value on the field {0} must be greater than 0 and less 15.")]
        [Display(Name = "Iterations")]
        public int? Iterations { get; set; }

        [Display(Name = "Replace Data")]
        public bool? ReplaceData { get; set; }
    }
}