using System.Net.Http.Json;
using System.Text.Json;
using AutoMapper;
using Cross.Cutting.Helper;
using Domain.Entities;
using Domain.ExternalServices;
using Infrastructure.DTO;

namespace Infrastructure.ExternalServices
{
    public class GetLaunches : IRequestLaunchService
    {
        private readonly IHttpClientFactory _client;
        private readonly IMapper _mapper;
        public GetLaunches(IHttpClientFactory client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
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
                if (!dataList.Results.Any())
                    throw new KeyNotFoundException(ErrorMessages.NoDataFromSpaceDevApi);

                var launches = _mapper.Map<List<Launch>>(dataList.Results);
                return launches;
            }
            catch(Exception ex)
            {
                //Needs implementate the exception handling for generate logs;
                throw new NotImplementedException();
            }
        }

        public async Task<Launch> RequestLaunchById(Guid id)
        {
            using var client = _client.CreateClient();
            try
            {
                string url = $"{EndPoints.TheSpaceDevsLaunchEndPoint}{id}";
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"{response.StatusCode} - {ErrorMessages.LaunchApiEndPointError}");
                
                var updatedLaunch = await response.Content.ReadFromJsonAsync<LaunchDTO>();
                if(ObjectHelper.IsObjectEmpty(updatedLaunch))
                    throw new JsonException(ErrorMessages.DeserializingContentError);

                var launch = _mapper.Map<Launch>(updatedLaunch);
                return launch;
            }
            catch(Exception ex)
            {
                //Needs implementate the exception handling for generate logs;
                throw new NotImplementedException();
            }
        }
    }
}