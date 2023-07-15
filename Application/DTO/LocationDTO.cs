﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.DTO
{
    public class LocationDTO
    {
        [Key]
        [Range(0, int.MaxValue)]
        [Display(Name = "ID")]
        [Required(ErrorMessage = "Atention! The ID field can't be null.")]
        public int Id { get; set; }

        [Display(Name = "Link")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid link.", MinimumLength = 2)]
        public string Url { get; set; }

        [Display(Name = "Slug")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Slug.", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Country Code")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Country Code.", MinimumLength = 2)]
        public string CountryCode { get; set; }

        [Display(Name = "Map Image")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Map Image link.", MinimumLength = 2)]
        [DataType(DataType.ImageUrl)]
        public string MapImage { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Total Launch Count")]
        public int TotalLaunchCount { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Total Landing Count")]
        public int TotalLandingCount { get; set; }

    }
}
