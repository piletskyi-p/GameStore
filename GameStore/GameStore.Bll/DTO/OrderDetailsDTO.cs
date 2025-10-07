namespace GameStore.Bll.DTO
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }

        public string ProductID { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }

        public int OrderId { get; set; }
    }
}
