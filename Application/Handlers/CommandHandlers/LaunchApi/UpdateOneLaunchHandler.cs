using System.Linq.Expressions;
using Application.Shared.Handler;
using Application.Wrappers;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Domain.Commands.Launch.Responses;
using Domain.Entities;
using Domain.ExternalServices;
using Domain.Handlers;
using Domain.Interface;
using Domain.Shared.Request;
using MediatR;

namespace Application.Handlers.CommandHandlers.LaunchApi
{
    public class UpdateOneLaunchHandler : BaseUpdateDataHandler, IRequestHandler<MediatrRequestWrapper<LaunchByIdRequest, UpdateOneLaunchResponse>, UpdateOneLaunchResponse>, IUpdateOneLaunchHandler
    {
        private readonly ILaunchRepository _launchRepository;
        private readonly IRequestLaunchService _request;
        private readonly ILaunchViewRepository _launchViewRepository;
        public UpdateOneLaunchHandler(
            ILaunchRepository launchRepository,
            IGenericDapperRepository genericDapperRepository,
            IRequestLaunchService request,
            ILaunchViewRepository launchViewRepository)
            : base(launchRepository, genericDapperRepository)
        {
            _request = request;
            _launchViewRepository = launchViewRepository;
        }
        public async Task<UpdateOneLaunchResponse> Handle(MediatrRequestWrapper<LaunchByIdRequest, UpdateOneLaunchResponse> request, CancellationToken cancellationToken)
        {
            var domainRequest = request.DomainRequest;
            return await Handle(domainRequest, cancellationToken);
        }

        public async Task<UpdateOneLaunchResponse> Handle(LaunchByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _ = request?.launchId ?? throw new ArgumentNullException(ErrorMessages.NullArgument);

                Expression<Func<Launch, bool>> launchQuery = l => l.Id == request.launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();
                var apiGuid = await _launchRepository.GetSelected(
                    filter: launchQuery,
                    selectColumns: l => l.ApiGuid,
                    buildObject: l => l);

                if(apiGuid == Guid.Empty)
                    throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

                var launch = await _request.RequestLaunchById(apiGuid);
                await SaveLaunch(launch, true);
                await _launchViewRepository.RefreshView();

                var result = await _launchViewRepository.GetById(
                    l => l.Id == request.launchId
                    && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName());                
                    
                return new UpdateOneLaunchResponse(true, SuccessMessages.UpdateJob, result);
            }
            catch(Exception ex)
            {
                return new UpdateOneLaunchResponse(false, ex.Message, null);
            }
        }
    }
}