using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public abstract class Repository<T> where T : class
    {
        private readonly FutureSpaceContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(FutureSpaceContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual IList<T> GetAll(
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
                query = query.Include(includeProperty);

            if (orderBy != null)
                return orderBy.Compile()(query).ToList();
            else
                return query.ToList();
        }

        public virtual IList<T> GetMany(
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            string includedProperties = "",
            int? howMany = null)
        {
            IQueryable<T> query = _dbSet;

            if (howMany != null)
                query = query.Take(howMany.Value);

            if (filters != null)
            {
                foreach (var filter in filters)
                    query = query.Where(filter);
            }

            foreach (var includeProperty in includedProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (orderBy != null)
                return orderBy.Compile()(query).ToList();
            else
                return query.ToList();
        }

        public virtual IEnumerable<TResult> GetSelected<TResult>(
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includedProperties = "",
            int? howMany = null,
            Func<T, TResult> selectColumns = null) where TResult : class
        {
            IQueryable<T> query = _dbSet;

            if (howMany != null)
                query = query.Take(howMany.Value);

            if (filters != null)
            {
                foreach (var filter in filters)
                    query = query.Where(filter);

                if (orderBy != null)
                    orderBy(query).ToList();
            }
            foreach (var includeProperty in includedProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (selectColumns != null)
                return query.Select(selectColumns);
            else
                return query.Cast<TResult>();
        }


    }
}
