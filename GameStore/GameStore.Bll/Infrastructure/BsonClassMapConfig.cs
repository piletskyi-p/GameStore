using GameStore.Dal.Entities.Mongo;
using MongoDB.Bson.Serialization;

namespace GameStore.Bll.Infrastructure
{
    public static class BsonClassMapConfig
    {
        public static void Register()
        {
            BsonClassMap.RegisterClassMap<OrderMongo>(cm =>
            {
                cm.MapProperty(c => c.CustomerId).SetElementName("CustomerID");
                cm.MapProperty(c => c.EmployeeId).SetElementName("EmployeeID");
                cm.MapProperty(c => c.OrderDateStr).SetElementName("OrderDate");
                cm.MapProperty(c => c.RequiredDate);
                cm.MapProperty(c => c.ShippedDateStr).SetElementName("ShippedDate");
                cm.MapProperty(c => c.Id).SetElementName("OrderID");
            });

            BsonClassMap.RegisterClassMap<BaseEntityMongo>(cm =>
            {
                cm.SetIdMember(cm.MapProperty(c => c._id));
            });
        }
    }
}