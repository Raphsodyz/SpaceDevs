using Application.Shared.Handler;
using Application.Wrappers;
using Cross.Cutting.Helper;
using Domain.Commands.Launch.Requests;
using Domain.Commands.Launch.Responses;
using Domain.ExternalServices;
using Domain.Handlers;
using Domain.Interface;
using MediatR;

namespace Application.Handlers.CommandHandlers.LaunchApi
{
    public class UpdateDataSetHandler : BaseUpdateDataHandler, IRequestHandler<MediatrRequestWrapper<UpdateLaunchSetRequest, UpdateDataSetResponse>, UpdateDataSetResponse>, IUpdateDataSetHandler
    {
        private readonly IRequestLaunchService _request;
        private readonly ILaunchViewRepository _launchViewRepository;
        public UpdateDataSetHandler(
            ILaunchRepository launchRepository,
            IGenericDapperRepository genericDapperRepository,
            IRequestLaunchService request,
            ILaunchViewRepository launchViewRepository,
            IUpdateLogRepository updateLogRepository)
            : base(launchRepository, genericDapperRepository, updateLogRepository)
        {
            _request = request;
            _launchViewRepository = launchViewRepository;
        }

        public async Task<UpdateDataSetResponse> Handle(MediatrRequestWrapper<UpdateLaunchSetRequest, UpdateDataSetResponse> request, CancellationToken cancellationToken)
        {
            var domainRequest = request.DomainRequest;
            return await Handle(domainRequest, cancellationToken);
        }

        public async Task<UpdateDataSetResponse> Handle(UpdateLaunchSetRequest request, CancellationToken cancellationToken)
        {
            request.Limit ??= 100;
            request.Iterations ??= 15;
            int offset = request.Skip ??= 0, entityCounter = 0, max = offset + ((int)request.Iterations * (int)request.Limit);

            try
            {
                for(int i = offset; i < max; i += (int)request.Limit)
                {
                    var launches = await _request.RequestLaunchSet((int)request.Limit, offset, entityCounter);
                    foreach(var data in launches)
                    {
                        await SaveLaunch(data, request.ReplaceData ?? false);
                        entityCounter++;
                    }

                    await GenerateLog(offset, SuccessMessages.PartialImportSuccess, entityCounter, true);
                    entityCounter = 0;
                    offset += (int)request.Limit;
                }
            }
            catch(Exception ex)
            {
                await GenerateLog(offset, ex.Message, entityCounter, false);
                throw;
            }
            finally
            {
                await _launchViewRepository.RefreshView();
            }

            await GenerateLog(offset, SuccessMessages.ImportedDataSuccess, entityCounter, true);
            return new UpdateDataSetResponse(true, SuccessMessages.ImportedDataSuccess);
        }
    }
}