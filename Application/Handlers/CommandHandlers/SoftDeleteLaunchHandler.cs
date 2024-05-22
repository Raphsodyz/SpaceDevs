using System.Linq.Expressions;
using Application.Wrappers;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Domain.Commands.Launch.Responses;
using Domain.Entities;
using Domain.Handlers;
using Domain.Interface;
using Domain.Shared.Request;
using MediatR;

namespace Application.Handlers.CommandHandlers
{
    public class SoftDeleteLaunchHandler : IRequestHandler<MediatrRequestWrapper<LaunchByIdRequest, SoftDeleteLaunchResponse>, SoftDeleteLaunchResponse>, ISoftDeleteLaunchHandler
    {
        private readonly IUnitOfWork _uow;
        public SoftDeleteLaunchHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<SoftDeleteLaunchResponse> Handle(MediatrRequestWrapper<LaunchByIdRequest, SoftDeleteLaunchResponse> request, CancellationToken cancellationToken)
        {
            var domainRequest = request.DomainRequest;
            return await Handle(domainRequest, cancellationToken);
        }

        public async Task<SoftDeleteLaunchResponse> Handle(LaunchByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _ = request?.launchId ?? throw new ArgumentNullException(ErrorMessages.NullArgument);
                ILaunchRepository _launchRepository = _uow.Repository(typeof(ILaunchRepository)) as ILaunchRepository;

                List<Expression<Func<Launch, bool>>> launchQuery = new()
                { l => l.Id == request.launchId && l.EntityStatus == EStatus.PUBLISHED.GetDisplayName() };
                var launchExists = await _launchRepository.EntityExist(filter: launchQuery.FirstOrDefault());
                
                if(!launchExists)
                    throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

                Expression<Func<Launch, Launch>> updateColumns = l => new Launch()
                { EntityStatus = EStatus.TRASH.GetDisplayName() };
                await _launchRepository.UpdateOnQuery(launchQuery, updateColumns);

                ILaunchViewRepository _launchViewRepository = _uow.Repository(typeof(ILaunchViewRepository)) as ILaunchViewRepository;
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