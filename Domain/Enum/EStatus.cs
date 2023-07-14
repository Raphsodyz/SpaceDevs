using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
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
