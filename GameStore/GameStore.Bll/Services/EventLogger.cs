using System;
using GameStore.Bll.Interfaces;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Mongo;
using GameStore.Dal.Interfaces;
using MongoDB.Bson;

namespace GameStore.Bll.Services
{
    public class EventLogger : IEventLogger
    {
        private readonly IMongoRepository<LogEntity<BaseEntity>> _mongoRepositoty;

        public EventLogger(IMongoRepository<LogEntity<BaseEntity>> mongoRepositoty)
        {
            _mongoRepositoty = mongoRepositoty;
        }

        public void LogCreate(BaseEntity entity)
        {
            var log = new LogEntity<BaseEntity>
            {
                Date = DateTime.UtcNow,
                Action = "Create",
                EntityType = entity.GetType().ToString(),
                Entity = entity,
                _id = ObjectId.GenerateNewId()
            };
            _mongoRepositoty.Create(log);
        }

        public void LogDelete(BaseEntity entity)
        {
            var log = new LogEntity<BaseEntity>
            {
                Date = DateTime.UtcNow,
                Action = "Delete",
                EntityType = entity.GetType().ToString(),
                Entity = entity,
                _id = ObjectId.GenerateNewId()
            };
            _mongoRepositoty.Create(log);
        }

        public void LogUpdate(BaseEntity entityOld, BaseEntity entityNew)
        {
            var log = new LogEntity<BaseEntity>
            {
                Date = DateTime.UtcNow,
                Action = "Update",
                EntityType = entityOld.GetType().ToString(),
                Entity = entityOld,
                EntityIfUpdate = entityNew,
                _id = ObjectId.GenerateNewId()
            };
            _mongoRepositoty.Create(log);
        }
    }
}
