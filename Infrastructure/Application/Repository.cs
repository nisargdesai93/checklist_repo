using Infrastructure.Entity;
using Infrastructure.ORM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Application.Implementation
{
    public abstract class BaseRepository
    {
        protected CheckListDbContext DbContext { get; set; }
    }
    public class Repository<T> : BaseRepository where T : DomainBase
    {
        protected DbSet<T> DbSet { get { return DbContext.Set<T>(); } }

        public Repository(CheckListDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public IQueryable<T> List => DbSet.AsQueryable();

        public IEnumerable<T> Fetch(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression).ToArray();
        }

        public T Get(long id)
        {
            return DbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return DbSet.SingleOrDefault(expression);
        }

        public long GetCount(Expression<Func<T, bool>> expression)
        {
            return DbSet.Count(expression);
        }

        public void Insert(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Added;
            DbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            var original = DbSet.Find(entity.Id);

            DbContext.Entry(original).CurrentValues.SetValues(entity);
            DbContext.Entry(original).State = EntityState.Modified;

            DbContext.SaveChanges();
        }

        public void Delete(T domain)
        {
            if (DbContext.Entry(domain).State == EntityState.Detached)
            {
                DbSet.Attach(domain);
            }

            DbContext.Remove(domain);
            DbContext.SaveChanges();
        }

        public bool Exists(Expression<Func<T, bool>> expression)
        {
            bool flag;
            flag = DbSet.Any(expression);
            return flag;
        }
    }
}
