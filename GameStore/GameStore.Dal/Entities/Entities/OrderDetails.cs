namespace GameStore.Dal.Entities
{
    public class OrderDetails : BaseEntity
    {
        public OrderDetails()
        {
        }

        public string ProductID { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }

        public int OrderId { get; set; }
    }
}