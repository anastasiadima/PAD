using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBroker.Interfaces
{
     public interface IRepository<TEntity, TKey>
     {
          IEnumerable<TEntity> GetAll();
          TEntity GetById(TKey id);
          TEntity Insert(TEntity entity);
          void Delete(int id);
          void Update(TEntity entity);
          void Save();
     }
}
