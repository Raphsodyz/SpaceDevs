using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    [Table("location")]
    public class Location : BaseEntity
    {
        [Column("url")]
        public string? Url { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("country_code")]
        public string? CountryCode { get; set; }
        [Column("map_image")]
        public string? MapImage { get; set; }
        [Column("total_launch_count")]
        public int? TotalLaunchCount { get; set; }
        [Column("total_landing_count")]
        public int? TotalLandingCount { get; set; }
        [Column("search")]
        [JsonIgnore]
        public string? Search { get; set; }
    }
}
