using System.Collections.Generic;
using System.Linq;

namespace Fame.Service.Services
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        TEntity GetById(object id);
        void Insert(TEntity entity);
        void Delete(TEntity entityToDelete);
        void DeleteAll();
        void Update(TEntity entity);
        void Delete(long id);
        IEnumerable<TEntity> Get(int page = 1, int pageSize = 10);
        bool Exists(string id);
    }
}