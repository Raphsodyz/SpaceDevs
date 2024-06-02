using Domain.Interface;
using Domain.Materializated.Views;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class LaunchViewRepository : GenericViewRepository<LaunchView>, ILaunchViewRepository
    {
        public LaunchViewRepository(FutureSpaceQueryContext context):base(context)
        {
            
        }

        public async Task<bool> ViewExists()
        {
            return await _context.Database.SqlQuery<bool>($"SELECT matviewname FROM pg_matviews WHERE matviewname = 'launch_view'").AnyAsync();
        }

        public async Task RefreshView()
        {
            _ = await _context.Database.ExecuteSqlRawAsync("REFRESH MATERIALIZED VIEW launch_view");
            return;
        }
    }
}