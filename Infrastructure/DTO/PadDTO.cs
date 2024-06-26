﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Infrastructure.DTO
{
    public class PadDTO
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

        [Range(0, int.MaxValue)]
        [Display(Name = "Agency")]
        public int? Agency_Id { get; set; }

        [Display(Name = "Name")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Name.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Info URL")]
        public string Info_Url { get; set; }

        [Display(Name = "Wiki URL")]
        public string Wiki_Url { get; set; }

        [Display(Name = "Map URL")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Map URL.", MinimumLength = 2)]
        public string Map_Url { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Latitude")]
        public double Latitude { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Longitude")]
        public double Longitude { get; set; }

        public LocationDTO Location { get; set; }

        [Display(Name = "Map Image")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Map Image link.", MinimumLength = 2)]
        [DataType(DataType.ImageUrl)]
        public string Map_Image { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Total Launch Count")]
        public int Total_Launch_Count { get; set; }

    }
}
