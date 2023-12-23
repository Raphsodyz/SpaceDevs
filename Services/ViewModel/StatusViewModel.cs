using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ViewModel
{
    public class StatusViewModel
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "The field {0} can't be null.")]
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Name.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Abbrev")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Abbrev.", MinimumLength = 2)]
        public string Abbrev { get; set; }

        [Display(Name = "Description")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Description.", MinimumLength = 2)]
        public string Description  { get; set; }
    }
}