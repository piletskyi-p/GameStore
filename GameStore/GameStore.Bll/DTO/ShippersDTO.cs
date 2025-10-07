using MongoDB.Bson;

namespace GameStore.Bll.DTO
{
    public class ShippersDTO
    {
        public ObjectId _id { get; set; }

        public int ShipperID { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }
    }
}