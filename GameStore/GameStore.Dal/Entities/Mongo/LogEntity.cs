using System;

namespace GameStore.Dal.Entities.Mongo
{
    public class LogEntity<TEntity> : BaseEntityMongo where TEntity : class 
    {
        public DateTime Date { get; set; }

        public string Action { get; set; }

        public string EntityType { get; set; }

        public TEntity Entity { get; set; }

        public TEntity EntityIfUpdate { get; set; }
    }
}
