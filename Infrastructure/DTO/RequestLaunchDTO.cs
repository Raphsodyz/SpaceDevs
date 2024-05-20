namespace Infrastructure.DTO
{
    public class RequestLaunchDTO
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<LaunchDTO> Results { get; set; }
    }
}