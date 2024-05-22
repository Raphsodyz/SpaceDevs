using System.Linq.Expressions;
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

namespace Application.Handlers.CommandHandlers
{
    public class UpdateOneLaunchHandler : IRequestHandler<MediatrRequestWrapper<LaunchByIdRequest, UpdateOneLaunchResponse>, UpdateOneLaunchResponse>, IUpdateOneLaunchHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestLaunchService _request;
        public UpdateOneLaunchHandler(IUnitOfWork uow, IRequestLaunchService request)
        {
            _uow = uow;
            _request = request;
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
                ILaunchRepository _launchRepository = _uow.Repository(typeof(ILaunchRepository)) as ILaunchRepository;

                Expression<Func<Launch, bool>> launchQuery = l => l.Id == request.launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName();
                var apiGuid = await _launchRepository.GetSelected(
                    filter: launchQuery,
                    selectColumns: l => l.ApiGuid,
                    buildObject: l => l);

                if(apiGuid == Guid.Empty)
                    throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

                var launch = await _request.RequestLaunchById(apiGuid);
            }
            catch(Exception ex)
            {
                return new UpdateOneLaunchResponse(false, ex.Message, null);
            }
        }
    }
}