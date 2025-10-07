namespace GameStore.Dal.Entities.Mongo
{
    public class OrderMongo : BaseEntityMongo
    {
        public int Id { get; set; } 

        public string CustomerId { get; set; }

        public int EmployeeId { get; set; }

        public string OrderDateStr { get; set; }

        public string RequiredDate { get; set; }

        public string ShippedDateStr { get; set; }
    }
}
