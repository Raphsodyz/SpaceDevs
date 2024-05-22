using System.Linq.Expressions;
using Application.Wrappers;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Domain.Handlers;
using Domain.Interface;
using Domain.Materializated.Views;
using Domain.Queries.Launch.Responses;
using Domain.Request;
using MediatR;

namespace Application.Handlers.QueryHandlers.Launch
{
    public class GetAllLaunchesPagedHandler : IRequestHandler<MediatrRequestWrapper<PageRequest, GetLaunchesPagedResponse>, GetLaunchesPagedResponse>, IGetAllLaunchesPagedHandler
    {
        private readonly IUnitOfWork _uow;
        public GetAllLaunchesPagedHandler(IUnitOfWork uow)
        {
            _uow = uow;
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
                ILaunchViewRepository _launchViewBusiness = _uow.Repository(typeof(ILaunchViewRepository)) as ILaunchViewRepository;
                List<Expression<Func<LaunchView, bool>>> publishedLaunchQuery = new(){ l => l.EntityStatus == EStatus.PUBLISHED.GetDisplayName() };
                
                var pagedResults = await _launchViewBusiness.GetViewPaged(
                    request?.Page ?? 0, 10,
                    filters: publishedLaunchQuery);

                if (!pagedResults.Entities.Any())
                    throw new KeyNotFoundException(ErrorMessages.NoData);
                
                return new GetLaunchesPagedResponse(true, string.Empty, pagedResults);
            }
            catch (Exception ex)
            {
                return new GetLaunchesPagedResponse(false, ex.Message, null);
            }
        }
    }
}