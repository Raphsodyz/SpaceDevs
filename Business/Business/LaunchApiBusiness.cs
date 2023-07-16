using Application.DTO;
using AutoMapper;
using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Domain.Enum;
using Domain.Helper;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using System.Net.Http.Json;

namespace Business.Business
{
    public class LaunchApiBusiness : BusinessBase<Launch, ILaunchRepository>, ILaunchApiBusiness, IBusiness
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public LaunchApiBusiness(IUnitOfWork uow,
            IConfiguration configuration,
            IMapper mapper):base(uow)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public Launch GetOneLaunch(int? launchId)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            Expression<Func<Launch, bool>> launchQuery = l => l.Id == launchId && l.status == EStatus.PUBLISHED.GetDisplayName();
            bool launchExist = _launchBusiness.GetSelected(filter: launchQuery, selectColumns: l => l.Id) > 0;
            if (!launchExist)
                throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            Launch launch = _launchBusiness.Get(
                filter: launchQuery,
                includedProperties: "Status, LaunchServiceProvider, Rocket.Configuration, Mission.Orbit, Pad.Location");

            return launch;
        }

        public Pagination<Launch> GetAllLaunchPaged(int? page)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            int totalEntities = _launchBusiness.EntityCount(l => l.status == EStatus.PUBLISHED.GetDisplayName());
            int totalPages = totalEntities % 10 == 0 ? totalEntities / 10 : (totalEntities / 10) + 1;
            if (page > totalPages)
                throw new InvalidOperationException($"{ErrorMessages.InvalidPageSelected} Total pages = {totalPages}");

            List<Expression<Func<Launch, bool>>> publishedLaunchQuery = new List<Expression<Func<Launch, bool>>>
            { l => l.status == EStatus.PUBLISHED.GetDisplayName() };
            var selectedPageLaunchList = _launchBusiness.GetAllPaged(
                page ?? 1, 10,
                filters: publishedLaunchQuery,
                includedProperties: "Status, LaunchServiceProvider, Rocket.Configuration, Mission.Orbit, Pad.Location",
                orderBy: l => l.OrderBy(la => la.Id));

            if (selectedPageLaunchList.Entities?.Count == 0)
                throw new KeyNotFoundException(ErrorMessages.NoData);

            return selectedPageLaunchList;
        }

        public void HardDeleteLaunch(int? launchId)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            Expression<Func<Launch, bool>> launchQuery = l => l.Id == launchId && l.status == EStatus.PUBLISHED.GetDisplayName();
            var launch = _launchBusiness.Get(filter: launchQuery) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);
            
            using var trans = _repository.GetTransaction();
            try
            {
                _launchBusiness.DeleteTransaction(launch);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public void SoftDeleteLaunch(int? launchId)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            Expression<Func<Launch, bool>> launchQuery = l => l.Id == launchId && l.status == EStatus.PUBLISHED.GetDisplayName();
            var launch = _launchBusiness.Get(filter: launchQuery) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            using var trans = _repository.GetTransaction();
            try
            {
                launch.status = EStatus.TRASH.GetDisplayName();
                _launchBusiness.SaveTransaction(launch);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public async Task<Launch> UpdateLaunch(int? launchId)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            Expression<Func<Launch, bool>> launchQuery = l => l.Id == launchId && l.status == EStatus.PUBLISHED.GetDisplayName();
            var launch = _launchBusiness.Get(filter: launchQuery) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            using var trans = _repository.GetTransaction();
            using HttpClient client = new HttpClient();
            try
            {
                string url = $"{_configuration.GetSection(EndPoints.TheSpaceDevsLaunchEndPoint).Value}{launch.ApiGuId}";
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"{response.StatusCode} - {ErrorMessages.LaunchApiEndPointError}");

                var updatedLaunch = await response.Content.ReadFromJsonAsync<LaunchDTO>() ?? throw new HttpRequestException(ErrorMessages.DeserializingEndPointContentError);
                if (updatedLaunch == null)
                    throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

                launch = _mapper.Map<Launch>(updatedLaunch);
                launch.status = EStatus.PUBLISHED.GetDisplayName();

                trans.Commit();
                return launch;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
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
                        launch.status = EStatus.DRAFT.GetDisplayName();
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
