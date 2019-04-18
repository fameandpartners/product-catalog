using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fame.Data.Repository;

namespace Fame.Service.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly IBaseRepository<TEntity> _repository;

        public BaseService(IBaseRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual TEntity GetById(object id)
        {
            return _repository.FindById(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _repository.Insert(entity);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            _repository.Delete(entityToDelete);
        }

        public virtual void Update(TEntity entity)
        {
            _repository.Update(entity);
        }

        public virtual void Delete(long id)
        {
            _repository.Delete(id);
        }

        public void DeleteAll()
        {
            _repository.DeleteWhere();
        }

        public IEnumerable<TEntity> Get(int page = 1, int pageSize = 10)
        {
            var skip = Math.Max(0, page - 1) * pageSize;
            return _repository.Get().Skip(skip).Take(pageSize).ToList();
        }

        public bool Exists(string id)
        {
            return _repository.FindById(id) != null;
        }
    }
}