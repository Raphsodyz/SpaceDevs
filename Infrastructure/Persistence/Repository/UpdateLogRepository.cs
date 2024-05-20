using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class UpdateLogRepository : GenericRepository<UpdateLog>, IUpdateLogRepository
    {
        public UpdateLogRepository(FutureSpaceContext context):base(context)
        {        
        }

        public async Task<int> LastOffSet()
        {
            IQueryable<UpdateLog> query = _dbSet;
            
            query = query.OrderByDescending(u => u.TransactionDate);
            var result = await query.FirstOrDefaultAsync();
            return result.OffSet;
        }
    }
}
