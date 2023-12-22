using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.DTO
{
    public class StatusDTO
    {
        [Key]
        [Range(1, int.MaxValue)]
        [Display(Name = "ID")]
        [JsonPropertyName("id")]
        [Required(ErrorMessage = "The field {0} can't be null.")]
        public int IdFromApi { get; set; }

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
