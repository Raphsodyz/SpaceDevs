using Data.Interface;
using Domain.Entities;
using Cross.Cutting.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Data.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        const int maxEntityReturn = 10;
        public DbContext _context;
        public DbSet<T> _dbSet;

        public async Task<IDbContextTransaction> GetTransaction()
        {
            return await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
        }

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IList<T>> GetAll(
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            string includedProperties = "",
            int? howMany = null)
        {
            IQueryable<T> query = _dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                    query = query.Where(filter);
            }

            query = query.Take(howMany ?? maxEntityReturn);

            foreach (var includeProperty in includedProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty.TrimStart());

            if (orderBy != null)
                return await orderBy.Compile()(query).ToListAsync();
            else
                return await query.ToListAsync();
        }

        public async Task<IList<T>> GetMany(
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            string includedProperties = "",
            int? howMany = null)
        {
            IQueryable<T> query = _dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                    query = query.Where(filter);
            }

            query = query.Take(howMany ?? maxEntityReturn);

            foreach (var includeProperty in includedProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty.TrimStart());

            if (orderBy != null)
                return await orderBy.Compile()(query).ToListAsync();
            else
                return await query.ToListAsync();
        }

        public async Task<IEnumerable<TResult>> GetAllSelectedColumns<TResult>(
            Func<T, TResult> selectColumns,
            IEnumerable<Expression<Func<T, bool>>> filters,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includedProperties = "",
            int? howMany = null)
        {
            IQueryable<T> query = _dbSet;
            
            foreach (var filter in filters)
                query = query.Where(filter);

            query = query.Take(howMany ?? maxEntityReturn);

            if (orderBy != null)
                query = orderBy(query);

            foreach (var includeProperty in includedProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty.TrimStart());

            var result = await query.ToListAsync();
            return result.Select(selectColumns);
        }

        public async Task<Pagination<T>> GetAllPaged(int page, int pageSize,
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            string includedProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                    query = query.Where(filter);
            }

            foreach (var includeProperty in includedProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty.TrimStart());

            IList<T> results;

            var entityCount = await query.CountAsync();
            if ((entityCount / pageSize) + 1 < page) page = 1;
            if (orderBy != null)
            {
                var q1 = orderBy.Compile()(query);
                var q2 = q1.Skip(page * pageSize).Take(pageSize);
                results = await q2.ToListAsync();
            }
            else
            {
                orderBy = q => q.OrderBy(c => c.Id);
                var q1 = orderBy.Compile()(query);
                var q2 = q1.Skip(page * pageSize).Take(pageSize);
                results = await q2.ToListAsync();
            }

            var result = new Pagination<T>(results, (int)Math.Ceiling((decimal)entityCount / pageSize), page, entityCount);
            return result;
        }

        public async Task<int> EntityCount(Expression<Func<T, bool>> filter = null)
        {
            return await _dbSet.CountAsync(filter);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter, string includedProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includedProperties))
            {
                foreach (var includeProperty in includedProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.TrimStart());
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<TResult> GetSelected<TResult>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TResult>> selectColumns,
            string includedProperties = "")
        {
            IQueryable<T> query = _dbSet;

            query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includedProperties))
            {
                foreach (var includeProperty in includedProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.TrimStart());
            }

            return await query.Select(selectColumns).FirstOrDefaultAsync();
        }

        public async Task UpdateOnQuery(
            List<Expression<Func<T, bool>>> filters,
            Expression<Func<T, T>> updateColumns,
            string includedProperties = null)
        {
            IQueryable<T> query = _dbSet;

            foreach(var filter in filters)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includedProperties))
            {
                foreach (var includeProperty in includedProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.TrimStart());
            }
            
            await query.UpdateFromQueryAsync(updateColumns);
        }

        public async Task<bool> EntityExist(Expression<Func<T, bool>> filter, string includedProperties = null)
        {
            IQueryable<T> query = _dbSet;

            query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includedProperties))
            {
                foreach (var includeProperty in includedProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.TrimStart());
            }

            return await query.AnyAsync();
        }

        public async Task Save(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveTransaction(T entity)
        {
            if (entity.Id == Guid.Empty)
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    T dbEntity = await _dbSet.FindAsync(entity.Id);
                    _context.Entry(dbEntity).CurrentValues.SetValues(entity);
                }
                else if (_context.Entry(entity).State == EntityState.Unchanged)
                    _context.Entry(entity).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(T entity) 
        {
            T dbEntity = await _dbSet.FindAsync(entity.Id);

            if (_context.Entry(dbEntity).State == EntityState.Detached)
                _dbSet.Attach(dbEntity);

            _dbSet.Remove(dbEntity);
        }

        public async Task DeleteTransaction(T entity)
        {
            T dbEntity = await _dbSet.FindAsync(entity.Id);
            if(_context.Entry(dbEntity).State == EntityState.Detached)
                _dbSet.Attach(dbEntity);
            
            _dbSet.Remove(dbEntity);
            await _context.SaveChangesAsync();
        }
    }
}
