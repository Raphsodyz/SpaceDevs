using AutoMapper;
using Core.Database.Repository;
using Core.Domain.Entities;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Dapper;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repository
{
    public class LaunchRepository(BaseApiContext context) : GenericRepository<Launch>(context), ILaunchRepository
    {
        public async Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Launch, TResult>> selectColumns, string includedProperties = null)
        {
            IQueryable<Launch> query = _dbSet;

            if(!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(s => EF.Functions.ILike(s.Search, $"%{searchTerm}%"));

            if (!string.IsNullOrWhiteSpace(includedProperties))
                foreach (var includeProperty in includedProperties.Split (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.TrimStart());

            var selectedQuery = query.Select(selectColumns);
            var result = await selectedQuery.ToListAsync();

            return result;
        }

        public async Task SoftDeleteLaunchById(Guid launchId)
        {
            var builder = new SqlBuilder();
            
            var template = builder.AddTemplate(@"UPDATE public.LAUNCH /**set**/ WHERE launchId = @launchId AND EntityStatus = @EntityStatus",
                new { launchId = launchId, EntityStatus = EStatus.PUBLISHED.GetDisplayName()});
            builder.Set(@"EntityStatus = @EntityStatus", new { EntityStatus = EStatus.TRASH.GetDisplayName()});

            await Connection.ExecuteAsync(template.RawSql);
        }
    }
}
