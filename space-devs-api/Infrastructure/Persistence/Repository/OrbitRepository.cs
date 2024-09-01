using Core.Database.Repository;
using Core.Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class OrbitRepository(BaseApiContext context) : GenericRepository<Orbit>(context), IOrbitRepository
    {
    }
}
