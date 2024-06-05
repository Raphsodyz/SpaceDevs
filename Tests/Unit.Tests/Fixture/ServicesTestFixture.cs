using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Tests.Unit.Tests.Fixture
{
    public class ServicesTestFixture : IDisposable
    {
        public Mock<IMediator> Mediator { get; private set; }

        public ServicesTestFixture()
        {
            Mediator = new Mock<IMediator>();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}