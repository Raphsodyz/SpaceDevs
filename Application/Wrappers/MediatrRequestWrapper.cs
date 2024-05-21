using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Wrappers
{
    public class MediatrRequestWrapper<TRequest, TResponse> : IRequest<TResponse>
    {
        TRequest DomainRequest { get; }
        TResponse DomainResponse { get; }

        public MediatrRequestWrapper(TRequest domainRequest, TResponse domainResponse)
        {
            DomainRequest = domainRequest;
            DomainResponse = domainResponse;
        }
    }
}