using System;
using System.Collections.Generic;
using GameStore.Dal.Entities;

namespace GameStore.Bll.Observer
{
    public class OrderModel
    {
        public bool IsPaid { get; set; }

        public int CustomerId { get; set; }

        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; }

        public List<Game> Games { get; set; }
    }
}