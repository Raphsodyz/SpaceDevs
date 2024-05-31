using System.Linq.Expressions;
using Application.Wrappers;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Domain.Commands.Launch.Requests;
using Domain.Commands.Launch.Responses;
using Domain.Entities;
using Domain.Handlers;
using Domain.Interface;
using MediatR;

namespace Application.Handlers.CommandHandlers.LaunchApi
{
    public class SoftDeleteLaunchHandler : IRequestHandler<MediatrRequestWrapper<SoftDeleteLaunchRequest, SoftDeleteLaunchResponse>, SoftDeleteLaunchResponse>, ISoftDeleteLaunchHandler
    {
        private readonly ILaunchRepository _launchRepository;
        private readonly ILaunchViewRepository _launchViewRepository;
        public SoftDeleteLaunchHandler(ILaunchRepository launchRepository, ILaunchViewRepository launchViewRepository)
        {
            _launchRepository = launchRepository;
            _launchViewRepository = launchViewRepository;
        }

        public async Task<SoftDeleteLaunchResponse> Handle(MediatrRequestWrapper<SoftDeleteLaunchRequest, SoftDeleteLaunchResponse> request, CancellationToken cancellationToken)
        {
            var domainRequest = request.DomainRequest;
            return await Handle(domainRequest, cancellationToken);
        }

        public async Task<SoftDeleteLaunchResponse> Handle(SoftDeleteLaunchRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _ = request?.launchId ?? throw new ArgumentNullException(ErrorMessages.NullArgument);

                List<Expression<Func<Launch, bool>>> launchQuery = new()
                { l => l.Id == request.launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName() };
                var launchExists = await _launchRepository.EntityExist(filter: launchQuery.FirstOrDefault());
                
                if(!launchExists)
                    throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

                Expression<Func<Launch, Launch>> updateColumns = l => new Launch()
                { EntityStatus = EStatus.TRASH.GetDisplayName() };
                
                await _launchRepository.UpdateOnQuery(launchQuery, updateColumns);
                await _launchViewRepository.RefreshView();

                return new SoftDeleteLaunchResponse(true, SuccessMessages.DeletedEntity);
            }
            catch(Exception ex)
            {
                return new SoftDeleteLaunchResponse(false, ex.Message);
            }
        }
    }
}