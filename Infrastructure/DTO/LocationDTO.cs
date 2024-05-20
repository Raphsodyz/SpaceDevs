using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Infrastructure.DTO
{
    public class LocationDTO
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        
        [Key]
        [Range(1, int.MaxValue)]
        [Display(Name = "ID")]
        [JsonPropertyName("id")]
        [Required(ErrorMessage = "The field {0} can't be null.")]
        public int IdFromApi { get; set; }

        [Display(Name = "Link")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid link.", MinimumLength = 2)]
        public string Url { get; set; }

        [Display(Name = "Slug")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Slug.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Country Code")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Country Code.", MinimumLength = 2)]
        public string Country_Code { get; set; }

        [Display(Name = "Map Image")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Map Image link.", MinimumLength = 2)]
        [DataType(DataType.ImageUrl)]
        public string Map_Image { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Total Launch Count")]
        public int Total_Launch_Count { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Total Landing Count")]
        public int Total_Landing_Count { get; set; }

    }
}
