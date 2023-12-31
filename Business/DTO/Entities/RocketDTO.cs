using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business.DTO.Entities
{
    public class RocketDTO
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        
        [Key]
        [Range(1, int.MaxValue)]
        [Display(Name = "ID")]
        [JsonPropertyName("id")]
        [Required(ErrorMessage = "The field {0} can't be null.")]
        public int IdFromApi { get; set; }

        public ConfigurationDTO Configuration { get; set; }

    }
}
