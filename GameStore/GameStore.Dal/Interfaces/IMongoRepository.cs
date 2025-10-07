using System;
using System.Collections.Generic;
using GameStore.Dal.Entities.Mongo;

namespace GameStore.Dal.Interfaces
{
    public interface IMongoRepository<TEntity> : IBaseGenericRepository<TEntity>
        where TEntity : BaseEntityMongo
    {
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
    }
}
