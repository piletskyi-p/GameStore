using System;
using System.Collections.Generic;

namespace GameStore.Dal.Entities
{
   public class Order : BaseEntity
    {
        public Order()
        {
        }

        public bool IsPaid { get; set; }

        public string CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime ShippedDate { get; set; }

        public bool IsShipped { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
