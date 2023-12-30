using Data.Context;
using Data.Interface;
using Data.Materializated.Views;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class LaunchViewRepository : GenericViewRepository<LaunchView>, ILaunchViewRepository
    {
        public LaunchViewRepository(FutureSpaceContext context):base(context)
        {
            
        }

        public async Task<bool> ViewExists()
        {
            return await _context.Database.SqlQuery<bool>($"SELECT matviewname FROM pg_matviews WHERE matviewname = 'launch_view'").AnyAsync();
        }
    }
}