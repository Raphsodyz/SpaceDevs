using Core.Database.Repository;
using Core.Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repository
{
    public class StatusRepository(BaseApiContext context) : GenericRepository<Status>(context), IStatusRepository
    {
    }
}
