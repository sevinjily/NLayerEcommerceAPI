using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework
{
    public class EFRepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity>
        where TEntity : class,IEntity
        where TContext : DbContext,new()

    {
        public void Add(TEntity entity)
        {
            using var context = new TContext();
          var addEntity=context.Entry(entity);
            addEntity.State = EntityState.Added;
            context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
           using var context=new TContext();
            var deleteEntity=context.Entry(entity);
            deleteEntity.State = EntityState.Deleted;
            context.SaveChanges();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            using var context= new TContext();
            return context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public void Update(TEntity entity)
        {
          using var context= new TContext();
            var updateEntity=context.Entry(entity);
            updateEntity.State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
