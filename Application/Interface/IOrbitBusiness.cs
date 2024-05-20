using Application.Interface;
using Domain.Entities;
using Domain.Interface;

namespace Business.Interface
{
    public interface IOrbitBusiness : IBusinessBase<Orbit, IOrbitRepository>
    {
    }
}
