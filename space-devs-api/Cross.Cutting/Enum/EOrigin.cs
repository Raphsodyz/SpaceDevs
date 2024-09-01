using System.ComponentModel.DataAnnotations;

namespace Cross.Cutting.Enum
{
    public enum EOrigin
    {
        [Display(Name = "UPDATE JOB")]
        JOB,
        [Display(Name = "API UPDATE")]
        API_UPDATE
    }
}
