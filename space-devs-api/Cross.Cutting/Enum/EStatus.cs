using System.ComponentModel.DataAnnotations;

namespace Cross.Cutting.Enum
{
    public enum EStatus
    {
        [Display(Name = "DRAFT")]
        DRAFT,
        [Display(Name = "TRASH")]
        TRASH,
        [Display(Name = "PUBLISHED")]
        PUBLISHED
    }
}
