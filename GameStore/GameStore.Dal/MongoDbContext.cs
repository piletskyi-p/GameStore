using System;
using System.Collections.Generic;
using System.Configuration;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Mongo;
using GameStore.Dal.Interfaces;
using MongoDB.Driver;

namespace GameStore.Dal
{
    public class MongoDbContext : IMongoDbContext
    {
        private static readonly Dictionary<Type, string> TypeCollectionMap
            = new Dictionary<Type, string>
            {
                [typeof(OrderMongo)] = "orders",
                [typeof(LogEntity<BaseEntity>)] = "Logs",
                [typeof(Shippers)] = "shippers"
            };

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;

        public MongoDbContext()
        {
            var url = new MongoUrl(_connectionString);
            var client = new MongoClient(url);
            Database = client.GetDatabase(url.DatabaseName);
        }

        public IMongoDatabase Database { get; }

        public IMongoCollection<TEntity> GetCollection<TEntity>() 
        {
            return Database.GetCollection<TEntity>(TypeCollectionMap[typeof(TEntity)]);
        }
    }
}