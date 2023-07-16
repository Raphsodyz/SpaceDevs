using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("LAUNCH_SERVICE_PROVIDER")]
    public class LaunchServiceProvider : BaseEntity
    {
        [Column("URL")]
        public string? Url { get; set; }
        [Column("NAME")]
        public string? Name { get; set; }
        [Column("TYPE")]
        public string? Type { get; set; }
    }
}
