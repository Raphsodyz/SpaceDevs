using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("mission")]
    public class Mission : BaseEntity
    {
        [Column("launch_library_id")]
        public int? LaunchLibraryId { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("type")]
        public string? Type { get; set; }
        [Column("id_orbit")]
        public Guid? IdOrbit { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdOrbit))]
        public Orbit Orbit { get; set; }
        [Column("launch_designator")]
        public string? LaunchDesignator { get; set; }
        [Column("search")]
        [JsonIgnore]
        public string? Search { get; set; }
    }
}
