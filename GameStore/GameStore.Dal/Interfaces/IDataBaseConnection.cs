using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GameStore.Dal.Interfaces
{
    public interface IDataBaseConnection
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbEntityEntry Entry(object entity);
    }
}