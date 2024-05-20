using Domain.Entities;
using Domain.Interface;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repository
{
    public class ConfigurationRepository : GenericRepository<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(FutureSpaceContext context):base(context)
        {
            
        }

        public virtual async Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Configuration, TResult>> selectColumns, string includedProperties = null)
        {
            IQueryable<Configuration> query = _dbSet;

            if(!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(s => EF.Functions.ILike(s.Search, $"%{searchTerm}%"));

            if (!string.IsNullOrWhiteSpace(includedProperties))
                foreach (var includeProperty in includedProperties.Split (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.TrimStart());

            var selectedQuery = query.Select(selectColumns);
            var result = await selectedQuery.ToListAsync();

            return result;
        }
    }
}
