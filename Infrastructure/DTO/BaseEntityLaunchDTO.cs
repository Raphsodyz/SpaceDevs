namespace Infrastructure.DTO
{
    public class BaseEntityLaunchDTO
    {
        public Guid? Id { get; set; }
        public Guid? ApiGuid { get; set; }
        public DateTime? AtualizationDate { get; set; }
        public DateTime? ImportedT { get; set; }
        public string Status { get; set; }
    }
}