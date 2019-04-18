using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Fame.Data.Repository
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected internal FameContext Context;
        protected internal readonly DbSet<TEntity> DbSet;

        protected BaseRepository(FameContext context)
        {
            Context = context;
            Contract.Assert(Context != null);
            DbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Get()
        {
            return DbSet;
        }

        public virtual TEntity FindById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            var isDetached = Context.Entry(entityToUpdate).State == EntityState.Detached;
            if (isDetached) DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public void Detach(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }

        public void DeleteWhere(Expression<Func<TEntity, bool>> predicate = null)
        {
            DbSet.RemoveRange(predicate != null ? DbSet.Where(predicate) : DbSet);
        }
    }
}
