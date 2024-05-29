namespace Infrastructure.DTO
{
    public class BaseEntityDTO
    {
        public Guid? Id { get; set; }
        public int? IdFromApi { get; set; }
        public DateTime? AtualizationDate { get; set; }
        public DateTime? ImportedT { get; set; }
        public string Status { get; set; }

        public BaseEntityDTO()
        {
            
        }

        public BaseEntityDTO(Guid? id, int? idFromApi, DateTime? atualizationDate, DateTime? importedT, string status)
        {
            Id = id;
            IdFromApi = idFromApi;
            AtualizationDate = atualizationDate;
            ImportedT = importedT;
            Status = status;
        }
    }
}