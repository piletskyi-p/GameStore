using MongoDB.Driver;

namespace GameStore.Dal.Interfaces
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }
        IMongoCollection<TEntity> GetCollection<TEntity>();
    }
}