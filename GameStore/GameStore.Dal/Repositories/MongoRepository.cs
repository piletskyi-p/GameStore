using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Dal.Entities.Mongo;
using GameStore.Dal.Interfaces;
using MongoDB.Driver;

namespace GameStore.Dal.Repositories
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity>
        where TEntity : BaseEntityMongo
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoRepository(IMongoDbContext mongoDbContext)
        {
            _collection = mongoDbContext.GetCollection<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return _collection.AsQueryable().ToList();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _collection.AsQueryable().Where(predicate).ToList();
        }

        public void Create(TEntity entity)
        {
            _collection.InsertOne(entity);
        }
    }
}
