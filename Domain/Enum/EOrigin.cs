using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum EOrigin
    {
        [Display(Name = "UPDATE JOB")]
        JOB,
        [Display(Name = "API UPDATE")]
        API_UPDATE
    }
}
