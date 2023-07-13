using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("MISSION")]
    public class Mission : BaseEntity
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("LAUNCH_LIBRARY_ID")]
        public int LaunchLibraryId { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        [Column("ID_LAUNCH_DESIGNATOR")]
        public int IdLaunchDesignator { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdLaunchDesignator))]
        public LaunchDesignator LaunchDesignator { get; set; }
        [Column("TYPE")]
        public string Type { get; set; }
        [Column("ID_ORBIT")]
        public int IdOrbit { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdOrbit))]
        public Orbit Orbit { get; set; }

        public Mission()
        {
            Orbit = new Orbit();
            LaunchDesignator = new LaunchDesignator();
        }
    }
}
