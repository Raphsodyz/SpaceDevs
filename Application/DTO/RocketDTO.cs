using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.DTO
{
    public class RocketDTO
    {
        [Key]
        [Range(0, int.MaxValue)]
        [Display(Name = "ID")]
        [Required(ErrorMessage = "Atention! The ID field can't be null.")]
        public int Id { get; set; }

        public ConfigurationDTO Configuration { get; set; }

    }
}
