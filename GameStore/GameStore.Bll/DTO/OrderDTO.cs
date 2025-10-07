using System;
using System.Collections.Generic;

namespace GameStore.Bll.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }

        public bool IsPaid { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippedDate { get; set; }

        public bool IsShipped { get; set; }

        public ICollection<OrderDetailsDTO> OrderDetails { get; set; }
    }
}
