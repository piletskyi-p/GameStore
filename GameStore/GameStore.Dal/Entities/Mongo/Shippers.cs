namespace GameStore.Dal.Entities.Mongo
{
    public class Shippers : BaseEntityMongo
    {
        public int ShipperID { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }
    }
}