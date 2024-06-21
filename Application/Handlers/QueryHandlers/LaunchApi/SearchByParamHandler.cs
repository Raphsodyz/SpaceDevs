using System.Linq.Expressions;
using Application.Wrappers;
using Cross.Cutting.Helper;
using Domain.Handlers;
using Domain.Interface;
using Domain.Materializated.Views;
using Domain.Queries.Launch.Responses;
using Domain.Repository;
using Domain.Request;
using MediatR;

namespace Application.Handlers.QueryHandlers.LaunchApi
{
    public class SearchByParamHandler : IRequestHandler<MediatrRequestWrapper<SearchLaunchRequest, SeachByParamResponse>, SeachByParamResponse>, ISearchByParamHandler
    {
        private readonly IMissionRepository _missionRepository;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IPadRepository _padRepository;
        private readonly ILaunchRepository _launchRepository;
        private readonly ILaunchViewRepository _launchViewRepository;
        private readonly IRedisRepository _redisRepository;

        public SearchByParamHandler(
            IMissionRepository missionRepository,
            IConfigurationRepository configurationRepository,
            ILocationRepository locationRepository,
            IPadRepository padRepository,
            ILaunchRepository launchRepository,
            ILaunchViewRepository launchViewRepository,
            IRedisRepository redisRepository
        )
        {
            _missionRepository = missionRepository;
            _configurationRepository = configurationRepository;
            _locationRepository = locationRepository;
            _padRepository = padRepository;
            _launchRepository = launchRepository;
            _launchViewRepository = launchViewRepository;
            _redisRepository = redisRepository;
        }

        public async Task<SeachByParamResponse> Handle(MediatrRequestWrapper<SearchLaunchRequest, SeachByParamResponse> request, CancellationToken cancellationToken)
        {
            var domainRequest = request.DomainRequest;
            return await Handle(domainRequest, cancellationToken);
        }

        public async Task<SeachByParamResponse> Handle(SearchLaunchRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var cachedLaunchSearchResults = await _redisRepository.GetFromSearch(request);
                if(cachedLaunchSearchResults != null)
                    return new SeachByParamResponse(true, string.Empty, cachedLaunchSearchResults);

                List<Expression<Func<LaunchView, bool>>> query = new();
                if(!string.IsNullOrEmpty(request.Mission))
                {
                    var idsMission = await _missionRepository.ILikeSearch(searchTerm: request.Mission.Trim(), selectColumns: m => m.Id);
                    if(idsMission != null && idsMission.Any()) query.Add(l => idsMission.Contains((Guid)l.IdMission));
                }
                    
                if(!string.IsNullOrWhiteSpace(request.Rocket))
                {            
                    var idsRocket = await _configurationRepository.ILikeSearch(searchTerm: request.Rocket.Trim(), selectColumns: r => r.Id);
                    if(idsRocket != null && idsRocket.Any()) query.Add(l => idsRocket.Contains((Guid)l.Rocket.IdConfiguration));
                }
            
                if(!string.IsNullOrWhiteSpace(request.Location))
                {
                    var idsLocation = await _locationRepository.ILikeSearch(searchTerm: request.Location.Trim(), selectColumns: l => l.Id);
                    if(idsLocation != null && idsLocation.Any()) query.Add(l => idsLocation.Contains((Guid)l.Pad.IdLocation));
                }

                if(!string.IsNullOrWhiteSpace(request.Pad))
                {
                    var idsPad = await _padRepository.ILikeSearch(searchTerm: request.Pad.Trim(), selectColumns: p => p.Id);
                    if(idsPad != null && idsPad.Any()) query.Add(l => idsPad.Contains((Guid)l.IdPad));
                }

                if(!string.IsNullOrWhiteSpace(request.Launch))
                {
                    var idsLaunch = await _launchRepository.ILikeSearch(searchTerm: request.Launch.Trim(), selectColumns: l => l.Id);
                    if(idsLaunch != null && idsLaunch.Any()) query.Add(l => idsLaunch.Contains(l.Id));
                }

                if(!query.Any())
                    throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

                var found = await _launchViewRepository.GetViewPaged(request.Page ?? 0, 10, query);
                
                if(!found.Entities.Any())
                    throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

                await _redisRepository.SetSearchPagination(request, found);
                return new SeachByParamResponse(true, string.Empty, found);
            }
            catch
            {
                throw;
            }
        }
    }
}