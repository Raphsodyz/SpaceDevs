using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("configuration")]
    public class Configuration : BaseEntity
    {
        [Column("launch_library_id")]
        public int? LaunchLibraryId { get; set; }
        [Column("url")]
        public string? Url { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("family")]
        public string? Family { get; set; }
        [Column("full_name")]
        public string? FullName { get; set; }
        [Column("variant")]
        public string? Variant { get; set; }
        [Column("search")]
        [JsonIgnore]
        public string? Search { get; set; }
    }
}
