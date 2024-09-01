using MediatR;

namespace Application.Wrappers
{
    public class MediatrRequestWrapper<TRequest, TResponse>(TRequest domainRequest, TResponse domainResponse) : IRequest<TResponse>
    {
        public TRequest DomainRequest { get; init; } = domainRequest;
        public TResponse DomainResponse { get; init; } = domainResponse;
    }
}