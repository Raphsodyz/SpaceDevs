using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class LaunchDTO
    {
        [Key]
        [Required(ErrorMessage = "Atention! The ID field can't be null.")]
        public Guid Id { get; set; }

        [Display(Name = "Link")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid link.", MinimumLength = 2)]
        public string Url { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "LaunchLibrary")]
        public int? Launch_Library_Id { get; set; }

        [Display(Name = "Slug")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Slug.", MinimumLength = 2)]
        public string Slug { get; set; }

        [Display(Name = "Name")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Name.", MinimumLength = 2)]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Status")]
        public int IdStatus { get; set; }
        
        public StatusDTO Status { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "Net")]
        public DateTime Net { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Window End")]
        public DateTime Window_End { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Window Start")]
        public DateTime Window_Start { get; set; }

        [Display(Name = "In Hold")]
        public bool? Inhold { get; set; }

        [Display(Name = "TBD Time")]
        public bool? TbdTime { get; set; }

        [Display(Name = "TBD Date")]
        public bool? TbdDate { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Probability")]
        public int? Probability { get; set; }

        [Display(Name = "Hold Reason")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Hold Reason.", MinimumLength = 2)]
        public string HoldReason { get; set; }

        [Display(Name = "Fail Reason")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Fail Reason.", MinimumLength = 2)]
        public string FailReason { get; set; }

        [Display(Name = "HashTag")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid HashTag.", MinimumLength = 2)]
        public string Hashtag { get; set; }

        public LaunchServiceProvider Launch_Service_Provider { get; set; }

        public RocketDTO Rocket { get; set; }

        public MissionDTO Mission { get; set; }

        public PadDTO Pad { get; set; }

        [Display(Name = "Webcast Live")]
        public bool Webcast_Live { get; set; }

        [Display(Name = "Image")]
        [StringLength(500, ErrorMessage = "Atention! Write a valid Image link.", MinimumLength = 2)]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [Display(Name = "Infographic")]
        [StringLength(200, ErrorMessage = "Atention! Write a valid Infographic.", MinimumLength = 2)]
        public string Infographic { get; set; }

        public string? Programs { get; set; }

    }
}
