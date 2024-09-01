using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    [Table("launch_service_provider")]
    public class LaunchServiceProvider : BaseEntity
    {
        [Column("url")]
        public string? Url { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("type")]
        public string? Type { get; set; }
    }
}
