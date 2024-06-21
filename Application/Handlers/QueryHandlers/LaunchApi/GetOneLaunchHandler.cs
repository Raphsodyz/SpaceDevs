using System.Linq.Expressions;
using Application.Wrappers;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Domain.Handlers;
using Domain.Interface;
using Domain.Materializated.Views;
using Domain.Queries.Launch.Requests;
using Domain.Queries.Launch.Responses;
using Domain.Repository;
using MediatR;

namespace Application.Handlers.QueryHandlers.LaunchApi
{
    public class GetOneLaunchHandler : IRequestHandler<MediatrRequestWrapper<GetByIdRequest, GetOneLaunchResponse>, GetOneLaunchResponse>, IGetOneLaunchHandler
    {
        private readonly ILaunchViewRepository _launchViewRepository;
        private readonly IRedisRepository _redisRepository;
        public GetOneLaunchHandler(
            ILaunchViewRepository launchViewRepository,
            IRedisRepository redisRepository)
        {
            _launchViewRepository = launchViewRepository;
            _redisRepository = redisRepository;
        }

        public async Task<GetOneLaunchResponse> Handle(MediatrRequestWrapper<GetByIdRequest, GetOneLaunchResponse> request, CancellationToken cancellationToken)
        {
            var domainRequest = request.DomainRequest;
            return await Handle(domainRequest, cancellationToken);
        }

        public async Task<GetOneLaunchResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _ = request?.launchId ?? throw new ArgumentNullException(ErrorMessages.NullArgument);

                var cachedLaunchResult = await _redisRepository.GetLaunchById(request.launchId);
                if (cachedLaunchResult != null)
                    return new GetOneLaunchResponse(true, string.Empty, cachedLaunchResult);

                Expression<Func<LaunchView, bool>> launchQuery = l => l.Id == request.launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();
                if (!await _launchViewRepository.ViewExists())
                    throw new Exception(ErrorMessages.ViewNotExists);

                var launch = await _launchViewRepository.GetById(filter: launchQuery) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);
                if(launch != null)
                    await _redisRepository.SetLaunch(launch);

                return new GetOneLaunchResponse(true, string.Empty, launch);
            }
            catch
            {
                throw;
            }
        }
    }
}