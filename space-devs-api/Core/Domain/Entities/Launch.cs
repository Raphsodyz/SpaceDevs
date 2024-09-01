using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Domain.Entities
{
    [Table("launch")]
    public class Launch : BaseEntity
    {
        [Column("api_guid")]
        public Guid ApiGuid { get; set; }
        [Column("url")]
        public string? Url { get; set; }
        [Column("launch_library_id")]
        public int? LaunchLibraryId { get; set; }
        [Column("slug")]
        public string? Slug { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("id_status")]
        public Guid? IdStatus { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdStatus))]
        public Status Status { get; set; }
        [Column("net")]
        public DateTime? Net { get; set; }
        [Column("window_end")]
        public DateTime? WindowEnd { get; set; }
        [Column("window_start")]
        public DateTime? WindowStart { get; set; }
        [Column("inhold")]
        public bool? Inhold { get; set; }
        [Column("tbd_time")]
        public bool? TbdTime { get; set; }
        [Column("tbd_date")]
        public bool? TbdDate { get; set; }
        [Column("probability")]
        public int? Probability { get; set; }
        [Column("hold_reason")]
        public string? HoldReason { get; set; }
        [Column("fail_reason")]
        public string? FailReason { get; set; }
        [Column("hashtag")]
        public string? Hashtag { get; set; }
        [Column("id_launch_service_provider")]
        public Guid? IdLaunchServiceProvider { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdLaunchServiceProvider))]
        public LaunchServiceProvider LaunchServiceProvider { get; set; }
        [Column("id_rocket")]
        public Guid? IdRocket { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdRocket))]
        public Rocket Rocket { get; set; }
        [Column("id_mission")]
        public Guid? IdMission { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdMission))]
        public Mission Mission { get; set; }
        [Column("id_pad")]
        public Guid? IdPad { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdPad))]
        public Pad Pad { get; set; }
        [Column("web_cast_live")]
        public bool? WebcastLive { get; set; }
        [Column("image")]
        public string? Image { get; set; }
        [Column("infographic")]
        public string? Infographic { get; set; }
        [Column("programs")]
        public string? Programs { get; set; }
        [Column("search")]
        [JsonIgnore]
        public string? Search { get; set; }

        #region Constructors

        public Launch()
        {
            
        }

        public Launch(Launch launch, Guid? idStatus, Guid? idLaunchServiceProvider, Guid? idRocket, Guid? idMission, Guid? idPad)
        {
            ApiGuid = launch.ApiGuid;
            Url = launch.Url;
            LaunchLibraryId = launch.LaunchLibraryId;
            Slug = launch.Slug;
            Name = launch.Name;
            IdStatus = idStatus;
            Net = launch.Net;
            WindowEnd = launch.WindowEnd;
            WindowStart = launch.WindowStart;
            Inhold = launch.Inhold;
            TbdDate = launch.TbdDate;
            TbdTime = launch.TbdTime;
            Probability = launch.Probability;
            HoldReason = launch.HoldReason;
            FailReason = launch.FailReason;
            Hashtag = launch.Hashtag;
            IdLaunchServiceProvider = idLaunchServiceProvider;
            IdRocket = idRocket;
            IdMission = idMission;
            IdPad = idPad;
            WebcastLive = launch.WebcastLive;
            Image = launch.Image;
            Infographic = launch.Infographic;
            Programs = launch.Programs;
            IdFromApi = launch.IdFromApi;
        }

        #endregion
    }
}
