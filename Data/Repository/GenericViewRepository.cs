using System.Linq.Expressions;
using Cross.Cutting.Helper;
using Data.Interface;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Data.Repository
{
    public abstract class GenericViewRepository<T> : IGenericViewRepository<T> where T : class
    {
        const int maxEntityReturn = 10;
        public DbContext _context;
        public DbSet<T> _dbSet;
    
        public GenericViewRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IList<T>> GetAll(
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            int? howMany = null)
        {
            IQueryable<T> query = _dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                    query = query.Where(filter);
            }

            query = query.Take(howMany ?? maxEntityReturn);

            if (orderBy != null)
                return await orderBy.Compile()(query).ToListAsync();
            else
                return await query.ToListAsync();
        }

        public async Task<Pagination<T>> GetViewPaged(int page, int pageSize,
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null)
        {
            IQueryable<T> query = _dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                    query = query.Where(filter);
            }

            IList<T> results;

            var entityCount = await query.CountAsync();
            int totalPages = entityCount % pageSize == 0 ? (int)Math.Ceiling((decimal)entityCount / pageSize) : (int)Math.Ceiling((decimal)entityCount / pageSize) - 1;

            if (totalPages < page) throw new InvalidOperationException($"{ErrorMessages.InvalidPageSelected} Total pages = {totalPages}");
            if (orderBy != null)
            {
                var q1 = orderBy.Compile()(query);
                var q2 = q1.Skip(page * pageSize).Take(pageSize);
                results = await q2.ToListAsync();
            }
            else
            {
                orderBy = q => q.OrderBy(c => c);
                var q1 = orderBy.Compile()(query);
                var q2 = q1.Skip(page * pageSize).Take(pageSize);
                results = await q2.ToListAsync();
            }

            var result = new Pagination<T>(results, totalPages, page, entityCount);
            return result;
        }

        public async Task<TObject> GetSelected<TResult, TObject>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TResult>> selectColumns,
            Func<TResult, TObject> buildObject)
        {
            IQueryable<T> query = _dbSet;

            query = query.Where(filter);
            var selectedQuery = query.Select(selectColumns);
            var result = await selectedQuery.FirstOrDefaultAsync();

            return result != null ? buildObject(result) : default;
        }

        public async Task<IEnumerable<TObject>> GetAllSelectedColumns<TResult, TObject>(
            Expression<Func<T, TResult>> selectColumns,
            IEnumerable<Expression<Func<T, bool>>> filters,
            Func<TResult, TObject> buildObject,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? howMany = null)
        {
            IQueryable<T> query = _dbSet;
            
            foreach (var filter in filters)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            var selectedQuery = query.Select(selectColumns);

            selectedQuery = selectedQuery.Take(howMany ?? maxEntityReturn);

            var result = await selectedQuery.ToListAsync();
            return result.Select(buildObject);
        }

        public async Task<int> EntityCount(Expression<Func<T, bool>> filter = null)
        {
            return await _dbSet.CountAsync(filter);
        }
    }
}