using System.Linq.Expressions;
using Application.Wrappers;
using Core.CQRS.Commands.Launch.Requests;
using Core.CQRS.Commands.Launch.Responses;
using Core.Database.Repository;
using Core.Domain.Entities;
using Core.MediatR.Handlers;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using MediatR;

namespace Application.Handlers.CommandHandlers.LaunchApi
{
    public class SoftDeleteLaunchHandler(ILaunchRepository launchRepository, ILaunchViewRepository launchViewRepository) : IRequestHandler<MediatrRequestWrapper<SoftDeleteLaunchRequest, SoftDeleteLaunchResponse>, SoftDeleteLaunchResponse>, ISoftDeleteLaunchHandler
    {
        private readonly ILaunchRepository _launchRepository = launchRepository;
        private readonly ILaunchViewRepository _launchViewRepository = launchViewRepository;

        public async Task<SoftDeleteLaunchResponse> Handle(MediatrRequestWrapper<SoftDeleteLaunchRequest, SoftDeleteLaunchResponse> request, CancellationToken cancellationToken)
        {
            var domainRequest = request.DomainRequest;
            return await Handle(domainRequest, cancellationToken);
        }

        public async Task<SoftDeleteLaunchResponse> Handle(SoftDeleteLaunchRequest request, CancellationToken cancellationToken)
        {
            _ = request?.launchId ?? throw new ArgumentNullException(ErrorMessages.NullArgument);

            List<Expression<Func<Launch, bool>>> launchQuery = [l => l.Id == request.launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName()];
            var launchExists = await _launchRepository.EntityExist(filter: launchQuery.FirstOrDefault());
            
            if(!launchExists)
                throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            Expression<Func<Launch, Launch>> updateColumns = l => new Launch()
            { EntityStatus = EStatus.TRASH.GetDisplayName() };
            
            await _launchRepository.SoftDeleteLaunchById((Guid)request.launchId);
            await _launchViewRepository.RefreshView();

            return new SoftDeleteLaunchResponse(true, SuccessMessages.DeletedEntity);
        }
    }
}