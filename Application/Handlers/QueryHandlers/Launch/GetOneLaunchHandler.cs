using System.Linq.Expressions;
using Application.Wrappers;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Domain.Handlers;
using Domain.Interface;
using Domain.Materializated.Views;
using Domain.Queries.Launch.Responses;
using Domain.Shared.Request;
using MediatR;

namespace Application.Handlers.QueryHandlers.Launch
{
    public class GetOneLaunchHandler : IRequestHandler<MediatrRequestWrapper<LaunchByIdRequest, GetOneLaunchResponse>, GetOneLaunchResponse>, IGetOneLaunchHandler
    {
        private readonly IUnitOfWork _uow;
        public GetOneLaunchHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<GetOneLaunchResponse> Handle(MediatrRequestWrapper<LaunchByIdRequest, GetOneLaunchResponse> request, CancellationToken cancellationToken)
        {
            var domainRequest = request.DomainRequest;
            return await Handle(domainRequest, cancellationToken);
        }

        public async Task<GetOneLaunchResponse> Handle(LaunchByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _ = request?.launchId ?? throw new ArgumentNullException(ErrorMessages.NullArgument);
                ILaunchViewRepository _launchViewRepository = _uow.Repository(typeof(ILaunchViewRepository)) as ILaunchViewRepository;
            
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