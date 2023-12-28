using Data.Context;
using Data.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
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
