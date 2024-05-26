namespace Infrastructure.DTO
{
    public class BaseEntityLaunchData
    {
        public Guid Id { get; set; }
        public Guid ApiGuid { get; set; }
        public DateTime AtualizationDate { get; set; }
        public DateTime ImportedT { get; set; }
        public string Status { get; set; }
    }
}