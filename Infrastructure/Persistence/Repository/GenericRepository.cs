using Domain.Entities;
using Cross.Cutting.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Data.Common;
using Domain.Interface;
using Infrastructure.Persistence.Context.Factory;

namespace Infrastructure.Persistence.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        const int maxEntityReturn = 10;
        public IDbContextFactory _contexts;

        public GenericRepository(IDbContextFactory contexts)
        {
            _contexts = contexts;
        }

        public async Task BeginTransaction()
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransaction()
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            await _context.Database.CurrentTransaction.CommitAsync();
        }

        public async Task RollbackTransaction()
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            await _context.Database.CurrentTransaction.RollbackAsync();
        }

        public DbConnection GetEfConnection()
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            return _context.Database.GetDbConnection();
        }

        public DbTransaction GetCurrentlyTransaction()
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            if(_context?.Database?.CurrentTransaction == null)
                return null;

            var trans = _context.Database.CurrentTransaction;
            return trans.GetDbTransaction();
        }

        public void SetupForeignKey<TEntity>(TEntity entity, string foreignKeyName, Guid desiredFkValue)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            var foreignKeys = entityType.GetProperties()
                .FirstOrDefault(p => p.Name.Equals(foreignKeyName, StringComparison.OrdinalIgnoreCase));

            _ = foreignKeys ?? throw new ArgumentException(ErrorMessages.ForeignKeyNotFound);

            foreignKeys.PropertyInfo.SetValue(entity, desiredFkValue);
        }

        public async Task<IList<T>> GetAll(
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            string includedProperties = "",
            int? howMany = null)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceQuery);
            DbSet<T> _dbSet = _context.Set<T>();

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

        public async Task<IEnumerable<TObject>> GetAllSelectedColumns<TResult, TObject>(
            Expression<Func<T, TResult>> selectColumns,
            IEnumerable<Expression<Func<T, bool>>> filters,
            Func<TResult, TObject> buildObject,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includedProperties = "",
            int? howMany = null)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceQuery);
            DbSet<T> _dbSet = _context.Set<T>();

            IQueryable<T> query = _dbSet;
                    
            foreach (var filter in filters)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            foreach (var includeProperty in includedProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty.TrimStart());

            var selectedQuery = query.Select(selectColumns);

            selectedQuery = selectedQuery.Take(howMany ?? maxEntityReturn);

            var result = await selectedQuery.ToListAsync();
            return result.Select(buildObject);
        }

        public async Task<Pagination<T>> GetAllPaged(int page, int pageSize,
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            string includedProperties = "")
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceQuery);
            DbSet<T> _dbSet = _context.Set<T>();

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
                orderBy = q => q.OrderBy(c => c.Id);
                var q1 = orderBy.Compile()(query);
                var q2 = q1.Skip(page * pageSize).Take(pageSize);
                results = await q2.ToListAsync();
            }

            var result = new Pagination<T>(results, totalPages, page, entityCount);
            return result;
        }

        public async Task<int> EntityCount(Expression<Func<T, bool>> filter = null)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceQuery);
            DbSet<T> _dbSet = _context.Set<T>();

            if(filter is null)
                return await _dbSet.CountAsync();

            return await _dbSet.CountAsync(filter);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter, string includedProperties = "")
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceQuery);
            DbSet<T> _dbSet = _context.Set<T>();

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

        public async Task<TObject> GetSelected<TResult, TObject>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TResult>> selectColumns,
            Func<TResult, TObject> buildObject,
            string includedProperties = "")
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceQuery);
            DbSet<T> _dbSet = _context.Set<T>();

            IQueryable<T> query = _dbSet;

            query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includedProperties))
            {
                foreach (var includeProperty in includedProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.TrimStart());
            }

            var selectedQuery = query.Select(selectColumns);
            var result = await selectedQuery.FirstOrDefaultAsync();
            
            return result != null ? buildObject(result) : default;
        }

        public async Task UpdateOnQuery(
            List<Expression<Func<T, bool>>> filters,
            Expression<Func<T, T>> updateColumns)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            DbSet<T> _dbSet = _context.Set<T>();

            IQueryable<T> query = _dbSet;

            foreach(var filter in filters)
                query = query.Where(filter);
            
            await query.UpdateFromQueryAsync(updateColumns);
        }

        public async Task UpdateOnQuery(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, T>> updateColumns)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            DbSet<T> _dbSet = _context.Set<T>();

            IQueryable<T> query = _dbSet;

            query = query.Where(filter);
            await query.UpdateFromQueryAsync(updateColumns);
        }

        public async Task<bool> EntityExist(Expression<Func<T, bool>> filter)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceQuery);
            DbSet<T> _dbSet = _context.Set<T>();

            IQueryable<T> query = _dbSet;

            query = query.Where(filter);
            return await query.AnyAsync();
        }

        public async Task AddToChangeTracker(T entity)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            await _context.AddAsync(entity);
        }

        public async Task Save(T entity)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            DbSet<T> _dbSet = _context.Set<T>();

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
            await DeleteProccess(entity);
        }

        public async Task Delete(Guid id)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            DbSet<T> _dbSet = _context.Set<T>();

            T dbEntity = await _dbSet.FindAsync(id);
            await DeleteProccess(dbEntity);
        }

        private async Task DeleteProccess(T entity)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceCommand);
            DbSet<T> _dbSet = _context.Set<T>();
            
            if(_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
