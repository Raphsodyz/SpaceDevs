using Application.DTO;
using AutoMapper;
using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Domain.Enum;
using Domain.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Business.Business
{
    public class JobBusiness : BusinessBase<Launch, ILaunchRepository>, IJobBusiness, IBusiness
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public JobBusiness(IUnitOfWork uow,
            IConfiguration configuration,
            IMapper mapper):base(uow)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<bool> UpdateDataSet()
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            int limit = 100;
            int offset = 0;
            int max = 2000;

            for (int i = offset; i < max; i += limit)
            {
                using var trans = _repository.GetTransaction();
                using HttpClient client = new();
                try
                {
                    string url = $"{_configuration.GetSection(EndPoints.TheSpaceDevsLaunchEndPoint).Value}/?limit={limit}&offset={offset}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException($"{response.StatusCode} - {ErrorMessages.LaunchApiEndPointError}");

                    List<LaunchDTO> dataList = await response.Content.ReadFromJsonAsync<List<LaunchDTO>>() ?? throw new HttpRequestException(ErrorMessages.DeserializingEndPointContentError);
                    if (dataList?.Count == 0)
                        throw new InvalidOperationException(ErrorMessages.NoDataFromSpaceDevApi);

                    foreach (var data in dataList)
                    {
                        var launch = _mapper.Map<Launch>(data);
                        launch.EntityStatus = EStatus.DRAFT.GetDisplayName();
                        _launchBusiness.SaveTransaction(launch);
                    }

                    trans.Commit();
                    offset += limit;
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
                catch (InvalidOperationException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return true;
        }
    }
}
