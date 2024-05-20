namespace Domain.Materializated.Views
{
    public class LaunchView
    {
        public Guid Id { get; set; }
        public DateTime AtualizationDate { get; set; }
        public DateTime ImportedT { get; set; }
        public string EntityStatus { get; set; }
        public Guid ApiGuId { get; set; }
        public string? Url { get; set; }
        public int? LaunchLibraryId { get; set; }
        public string? Slug { get; set; }
        public string? Name { get; set; }
        public Guid? IdStatus { get; set; }
        public StatusView Status { get; set; }
        public DateTime? Net { get; set; }
        public DateTime? WindowEnd { get; set; }
        public DateTime? WindowStart { get; set; }
        public bool? Inhold { get; set; }
        public bool? TbdTime { get; set; }
        public bool? TbdDate { get; set; }
        public int? Probability { get; set; }
        public string? HoldReason { get; set; }
        public string? FailReason { get; set; }
        public string? Hashtag { get; set; }
        public Guid? IdLaunchServiceProvider { get; set; }
        public LaunchServiceProviderView LaunchServiceProvider { get; set; }
        public Guid? IdRocket { get; set; }
        public RocketView Rocket { get; set; }
        public Guid? IdMission { get; set; }
        public MissionView Mission { get; set; }
        public Guid? IdPad { get; set; }
        public PadView Pad { get; set; }
        public bool? WebcastLive { get; set; }
        public string? Image { get; set; }
        public string? Infographic { get; set; }
        public string? Programs { get; set; }
    }
}