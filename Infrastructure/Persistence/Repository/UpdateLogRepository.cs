using Cross.Cutting.Helper;
using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Context.Factory;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class UpdateLogRepository : GenericRepository<UpdateLog>, IUpdateLogRepository
    {
        public UpdateLogRepository(DbContextFactory contexts):base(contexts)
        {        
        }

        public async Task<int> LastOffSet()
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceQuery);
            DbSet<UpdateLog> _dbSet = _context.Set<UpdateLog>();

            IQueryable<UpdateLog> query = _dbSet;
            
            query = query.OrderByDescending(u => u.TransactionDate);
            var result = await query.FirstOrDefaultAsync();
            return result.OffSet;
        }
    }
}
