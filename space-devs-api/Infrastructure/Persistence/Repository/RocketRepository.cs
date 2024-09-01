using Core.Database.Repository;
using Core.Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class RocketRepository(BaseApiContext context) : GenericRepository<Rocket>(context), IRocketRepository
    {
    }
}
