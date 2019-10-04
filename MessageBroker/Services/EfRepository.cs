using MessageBroker.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageBroker.Services
{
     public class EfRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
     {
          private readonly DatabaseContext _context;

          public EfRepository(DatabaseContext context)
          {
               _context = context;
          }

          protected DbSet<TEntity> Set => _context.Set<TEntity>();

          public IEnumerable<TEntity> GetAll()
          {
               return Set.ToList();
          }

          public TEntity GetById(TKey id)
          {
               return Set.Find(id);
          }

          public TEntity Insert(TEntity entity)
          {
               Set.Add(entity);
               _context.SaveChanges();
               return entity;
          }

          public void Delete(int id)
          {
               var entity = Set.Find(id);
               Set.Remove(entity);
          }

          public void Update(TEntity entity)
          {
               Set.Update(entity);
          }

          public void Save()
          {
               _context.SaveChanges();
          }
     }
}
