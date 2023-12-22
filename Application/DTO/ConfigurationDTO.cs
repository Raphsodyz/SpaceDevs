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
    public class ConfigurationDTO
    {
        [Key]
        [Range(1, int.MaxValue)]
        [Display(Name = "ID")]
        [Required(ErrorMessage = "The field {0} can't be null.")]
        [JsonPropertyName("id")]
        public int IdFromApi { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Launch Library")]
        public int? Launch_Library_Id { get; set; }

        [Display(Name = "Link")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid link.", MinimumLength = 2)]
        public string Url { get; set; }

        [Display(Name = "Name")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Name.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Family")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Family.", MinimumLength = 2)]
        public string Family { get; set; }

        [Display(Name = "Full Name")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Full Name.", MinimumLength = 2)]
        public string full_name { get; set; }

        [Display(Name = "Variant")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Variant.", MinimumLength = 2)]
        public string Variant { get; set; }
    }
}
