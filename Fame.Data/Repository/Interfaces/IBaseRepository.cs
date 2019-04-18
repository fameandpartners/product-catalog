using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fame.Data.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get();
        TEntity FindById(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        void Detach(TEntity entity);
        void DeleteWhere(Expression<Func<TEntity, bool>> predicate = null);
    }
}
