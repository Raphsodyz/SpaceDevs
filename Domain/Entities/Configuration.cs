using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("CONFIGURATION")]
    public class Configuration : BaseEntity
    {
        [Column("LAUNCH_LIBRARY_ID")]
        public int? LaunchLibraryId { get; set; }
        [Column("URL")]
        public string Url { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("FAMILY")]
        public string Family { get; set; }
        [Column("FULL_NAME")]
        public string FullName { get; set; }
        [Column("VARIANT")]
        public string Variant { get; set; }
    }
}
