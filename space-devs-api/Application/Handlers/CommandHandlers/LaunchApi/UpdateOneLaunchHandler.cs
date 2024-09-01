using System.Linq.Expressions;
using Application.Wrappers;
using Core.CQRS.Commands.Launch.Requests;
using Core.CQRS.Commands.Launch.Responses;
using Core.ExternalServices;
using Core.MediatR.Handlers;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using MediatR;

namespace Application.Handlers.CommandHandlers.LaunchApi
{
    public class UpdateOneLaunchHandler(ISpaceDevsUpdateService service) : IRequestHandler<MediatrRequestWrapper<UpdateOneLaunchRequest, UpdateOneLaunchResponse>, UpdateOneLaunchResponse>, IUpdateOneLaunchHandler
    {
        private readonly ISpaceDevsUpdateService _service = service;
        public async Task<UpdateOneLaunchResponse> Handle(MediatrRequestWrapper<UpdateOneLaunchRequest, UpdateOneLaunchResponse> request, CancellationToken cancellationToken)
        {
            var domainRequest = request.DomainRequest;
            return await Handle(domainRequest, cancellationToken);
        }

        public async Task<UpdateOneLaunchResponse> Handle(UpdateOneLaunchRequest request, CancellationToken cancellationToken)
        {
            return await _service.UpdateLaunchById((Guid)request.launchId, cancellationToken);
        }
    }
}