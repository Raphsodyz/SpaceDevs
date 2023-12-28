using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("pad")]
    public class Pad : BaseEntity
    {
        [Column("url")]
        public string? Url { get; set; }
        [Column("agency_id")]
        public int? AgencyId { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("info_url")]
        public string? InfoUrl { get; set; }
        [Column("wiki_url")]
        public string? WikiUrl { get; set; }
        [Column("map_url")]
        public string? MapUrl { get; set; }
        [Column("latitude")]
        public double? Latitude { get; set; }
        [Column("longitude")]
        public double? Longitude { get; set; }
        [Column("id_location")]
        public Guid? IdLocation { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdLocation))]
        public Location Location { get; set; }
        [Column("map_image")]
        public string? MapImage { get; set; }
        [Column("total_launch_count")]
        public int? TotalLaunchCount { get; set; }
        [Column("search")]
        [JsonIgnore]
        public string? Search { get; set; }
    }
}
