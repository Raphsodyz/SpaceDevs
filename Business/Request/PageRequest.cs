using System.ComponentModel.DataAnnotations;
using Cross.Cutting.Helper;

namespace Business.Request
{
    public class PageRequest
    {
        [Display(Name = "Page")]
        [Range(0, int.MaxValue, ErrorMessage = "{0}: " + ErrorMessages.InvalidPageSelected)]
        [Required(ErrorMessage = "{0}: " + ErrorMessages.NullArgument)]
        public int? Page { get; set; }

        public PageRequest()
        {
            
        }

        public PageRequest(int? page)
        {
            Page = page;
        }
    }
}