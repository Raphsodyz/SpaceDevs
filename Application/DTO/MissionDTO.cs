using Domain.Entities;
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
    public class MissionDTO
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [Key]
        [Range(1, int.MaxValue)]
        [Display(Name = "ID")]
        [JsonPropertyName("id")]
        [Required(ErrorMessage = "The field {0} can't be null.")]
        public int IdFromApi { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "LaunchLibrary")]
        public int? Launch_Library_Id { get; set; }

        [Display(Name = "Name")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Name.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Description.", MinimumLength = 2)]
        public string Description { get; set; }

        [Display(Name = "Type")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Type.", MinimumLength = 2)]
        public string Type { get; set; }

        public OrbitDTO Orbit { get; set; }

    }
}
