using System.Linq.Expressions;
using Application.Wrappers;
using Cross.Cutting.Enum;
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
    public class GetAllLaunchesPagedHandler : IRequestHandler<MediatrRequestWrapper<PageRequest, GetLaunchesPagedResponse>, GetLaunchesPagedResponse>, IGetAllLaunchesPagedHandler
    {
        private readonly ILaunchViewRepository _launchViewRepository;
        private readonly IRedisRepository _redisRepository;
        public GetAllLaunchesPagedHandler(
            ILaunchViewRepository launchViewRepository,
            IRedisRepository redisRepository)
        {
            _launchViewRepository = launchViewRepository;
            _redisRepository = redisRepository;
        }

        public async Task<GetLaunchesPagedResponse> Handle(MediatrRequestWrapper<PageRequest, GetLaunchesPagedResponse> request, CancellationToken cancellationToken)
        {
            var domainRequest = request.DomainRequest;
            return await Handle(domainRequest, cancellationToken);
        }

        public async Task<GetLaunchesPagedResponse> Handle(PageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var cachedPaginatedLaunchResult = await _redisRepository.GetPagination(request?.Page ?? 0);
                if(cachedPaginatedLaunchResult != null)
                    return new GetLaunchesPagedResponse(true, string.Empty, cachedPaginatedLaunchResult);

                List<Expression<Func<LaunchView, bool>>> publishedLaunchQuery = new(){ l => l.EntityStatus == EStatus.PUBLISHED.GetDisplayName() };
                var pagedResults = await _launchViewRepository.GetViewPaged(
                    request?.Page ?? 0, 10,
                    filters: publishedLaunchQuery);

                if (!pagedResults.Entities.Any())
                    throw new KeyNotFoundException(ErrorMessages.NoData);
                
                await _redisRepository.SetPagination(request?.Page ?? 0, pagedResults);
                return new GetLaunchesPagedResponse(true, string.Empty, pagedResults);
            }
            catch
            {
                throw;
            }
        }
    }
}