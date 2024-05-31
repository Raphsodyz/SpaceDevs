using System.Linq.Expressions;
using Application.Wrappers;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Domain.Handlers;
using Domain.Interface;
using Domain.Materializated.Views;
using Domain.Queries.Launch.Requests;
using Domain.Queries.Launch.Responses;
using MediatR;

namespace Application.Handlers.QueryHandlers.LaunchApi
{
    public class GetOneLaunchHandler : IRequestHandler<MediatrRequestWrapper<GetByIdRequest, GetOneLaunchResponse>, GetOneLaunchResponse>, IGetOneLaunchHandler
    {
        private readonly ILaunchViewRepository _launchViewRepository;
        public GetOneLaunchHandler(ILaunchViewRepository launchViewRepository)
        {
            _launchViewRepository = launchViewRepository;
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
                Expression<Func<LaunchView, bool>> launchQuery = l => l.Id == request.launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();
                
                if (!await _launchViewRepository.ViewExists())
                    throw new Exception(ErrorMessages.ViewNotExists);

                var launch = await _launchViewRepository.GetById(filter: launchQuery) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);
                return new GetOneLaunchResponse(true, string.Empty, launch);
            }
            catch(Exception ex)
            {
                return new GetOneLaunchResponse(false, ex.Message, null);
            }
        }
    }
}