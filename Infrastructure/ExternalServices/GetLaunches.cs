using System.Net.Http.Json;
using Cross.Cutting.Helper;
using Domain.Entities;
using Domain.ExternalServices;
using Infrastructure.DTO;

namespace Infrastructure.ExternalServices
{
    public class GetLaunches : IRequestLaunchService
    {
        private readonly IHttpClientFactory _client;
        public GetLaunches(IHttpClientFactory client)
        {
            _client = client;
        }

        public async Task<List<Launch>> RequestLaunchSet(int limit, int offset)
        {
            using var client = _client.CreateClient();
            try
            {
                string url = $"{EndPoints.TheSpaceDevsLaunchEndPoint}?limit={limit}&offset={offset}";
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"{response.StatusCode} - {ErrorMessages.LaunchApiEndPointError}");

                RequestLaunchDTO dataList = await response.Content.ReadFromJsonAsync<RequestLaunchDTO>() ?? throw new HttpRequestException(ErrorMessages.DeserializingContentError);
                if ((bool)!dataList.Results?.Any())
                    throw new KeyNotFoundException(ErrorMessages.NoDataFromSpaceDevApi);
            }
            catch(Exception ex)
            {

            }
        }

        public Task<Launch> RequestLaunchById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}