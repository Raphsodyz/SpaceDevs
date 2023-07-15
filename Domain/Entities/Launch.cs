using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("LAUNCH")]
    public class Launch : BaseEntity
    {
        [Column("API_GUID")]
        public Guid ApiGuId { get; set; }
        [Column("URL")]
        public string Url { get; set; }
        [Column("LAUNCH_LIBRARY_ID")]
        public int? LaunchLibraryId { get; set; }
        [Column("SLUG")]
        public string Slug { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("ID_STATUS")]
        public int IdStatus { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdStatus))]
        public Status Status { get; set; }
        [Column("NET")]
        public DateTime Net { get; set; }
        [Column("WINDOW_END")]
        public DateTime WindowEnd { get; set; }
        [Column("WINDOW_START")]
        public DateTime WindowStart { get; set; }
        [Column("INHOLD")]
        public bool Inhold { get; set; }
        [Column("TBD_TIME")]
        public bool TbdTime { get; set; }
        [Column("TBD_DATE")]
        public bool TbdDate { get; set; }
        [Column("PROBABILITY")]
        public int? Probability { get; set; }
        [Column("HOLD_REASON")]
        public string HoldReason { get; set; }
        [Column("FAIL_REASON")]
        public string FailReason { get; set; }
        [Column("HASHTAG")]
        public string Hashtag { get; set; }
        [Column("ID_LAUNCH_SERVICE_PROVIDER")]
        public int IdLaunchServiceProvider { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdLaunchServiceProvider))]
        public LaunchServiceProvider LaunchServiceProvider { get; set; }
        [Column("ID_ROCKET")]
        public int IdRocket { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdRocket))]
        public Rocket Rocket { get; set; }
        [Column("ID_MISSION")]
        public int IdMission { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdMission))]
        public Mission Mission { get; set; }
        [Column("ID_PAD")]
        public int IdPad { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdPad))]
        public Pad Pad { get; set; }
        [Column("WEB_CAST_LIVE")]
        public bool WebcastLive { get; set; }
        [Column("IMAGE")]
        public string Image { get; set; }
        [Column("INFOGRAPHIC")]
        public string Infographic { get; set; }
        [Column("PROGRAMS")]
        public string[] Programs { get; set; }
    }
}
