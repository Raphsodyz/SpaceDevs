using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Infrastructure.DTO
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
