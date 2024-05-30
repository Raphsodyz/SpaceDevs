using System.Net.Http.Json;
using System.Text.Json;
using AutoMapper;
using Cross.Cutting.Helper;
using Domain.Entities;
using Domain.ExternalServices;
using Domain.Interface;
using Infrastructure.DTO;

namespace Infrastructure.ExternalServices
{
    public class GetLaunchesFromSpaceDevs : IRequestLaunchService
    {
        private readonly IHttpClientFactory _client;
        private readonly IMapper _mapper;
        private readonly IUpdateLogRepository _updateLogRepository;
        public GetLaunchesFromSpaceDevs(
            IHttpClientFactory client,
            IMapper mapper,
            IUpdateLogRepository updateLogRepository)
        {
            _client = client;
            _mapper = mapper;
            _updateLogRepository = updateLogRepository;
        }

        public async Task<List<Launch>> RequestLaunchSet(int limit, int offset, int entityCounter)
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
                await _updateLogRepository.Save(new UpdateLog(offset, ex.Message, entityCounter, false));
                throw;
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
                await _updateLogRepository.Save(new UpdateLog(0, ex.Message, 1, false));
                throw;
            }
        }
    }
}