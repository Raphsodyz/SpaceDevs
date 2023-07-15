using AutoMapper;
using Data.Context;
using Data.Interface;
using Domain.Entities;
using Domain.Helper;
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
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly FutureSpaceContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IMapper _mapper;

        public GenericRepository(FutureSpaceContext context)
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

        public virtual IEnumerable<TResult> GetAllSelectedColumns<TResult>(
            IEnumerable<Expression<Func<T, bool>>> filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includedProperties = "",
            int? howMany = null,
            Func<T, TResult> selectColumns = null)
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

        public virtual Pagination<T> GetAllPaged(int page, int pageSize,
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

            IList<T> results;

            var entityCount = query.Count();
            if ((entityCount / pageSize) + 1 < page) page = 1;
            if (orderBy != null)
            {
                var q1 = orderBy.Compile()(query);
                var q2 = q1.Skip((page - 1) * pageSize).Take(pageSize);
                results = q2.ToList();
            }
            else
            {
                orderBy = (q => q.OrderBy(c => c.Id));
                var q1 = orderBy.Compile()(query);
                var q2 = q1.Skip((page - 1) * pageSize).Take(pageSize);
                results = q2.ToList();
            }

            var result = new Pagination<T>(results, (int)Math.Ceiling((decimal)entityCount / pageSize), page, entityCount);
            return result;
        }

        public virtual int EntityCount(Expression<Func<T, bool>> filter = null)
        {
            return _dbSet.Count(filter);
        }

        public virtual T Get(Expression<Func<T, bool>> filter, string includedProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includedProperties))
            {
                foreach (var includeProperty in includedProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);
            }

            return query.FirstOrDefault();
        }

        public virtual TResult GetSelected<TResult>(
            Expression<Func<T, bool>> filter = null,
            string includedProperties = "",
            Func<T, TResult> selectColumns = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(includedProperties))
            {
                foreach (var includeProperty in includedProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);
            }

            TResult resultado;

            if (selectColumns != null)
                resultado = query.Select(selectColumns).FirstOrDefault();
            else
                resultado = query.Cast<TResult>().SingleOrDefault();

            return resultado;
        }

        public virtual void Save(T entity)
        {
            if (entity.Id == 0)
                _dbSet.Add(entity);
            
            if(_context.Entry(entity).State == EntityState.Detached)
            {
                T dbEntity = _dbSet.Find(entity.Id);
                _context.Entry(dbEntity).CurrentValues.SetValues(entity);
            }
            else if(_context.Entry(entity).State == EntityState.Unchanged)
                _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void SaveTransaction(T entity)
        {
            if (entity.Id == 0)
            {
                _dbSet.Add(entity);
                _context.SaveChanges();
                _mapper.Map(entity, entity);
            }
            else
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    T dbEntity = _dbSet.Find(entity.Id);
                    _context.Entry(dbEntity).CurrentValues.SetValues(entity);
                }
                else if (_context.Entry(entity).State == EntityState.Unchanged)
                    _context.Entry(entity).State = EntityState.Modified;

                _context.SaveChanges();
            }
        }

        public virtual void Delete(T entity) 
        {
            T dbEntity = _dbSet.Find(entity.Id);

            if (_context.Entry(dbEntity).State == EntityState.Detached)
                _dbSet.Attach(dbEntity);

            _dbSet.Remove(dbEntity);
        }

        public virtual void DeleteTransaction(T entity)
        {
            T dbEntity = _dbSet.Find(entity.Id);
            if(_context.Entry(dbEntity).State == EntityState.Detached)
                _dbSet.Attach(dbEntity);
            
            _dbSet.Remove(dbEntity);
            _context.SaveChanges();
        }
    }
}
