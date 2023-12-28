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
    public class ConfigurationRepository : GenericRepository<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(FutureSpaceContext context):base(context)
        {
            
        }

        public virtual async Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Func<Configuration, TResult> selectColumns, string includedProperties = null)
        {
            IQueryable<Configuration> query = _dbSet;

            if(!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(s => EF.Functions.ILike(s.Search, $"%{searchTerm}%"));

            if (!string.IsNullOrWhiteSpace(includedProperties))
                foreach (var includeProperty in includedProperties.Split (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.TrimStart());

            var result = await query.ToListAsync();
            return result.Select(selectColumns);
        }
    }
}
