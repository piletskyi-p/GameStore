using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels
{
    public class OrderViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime ShippedDate { get; set; }

        public bool IsShipped { get; set; }

        public bool IsPaid { get; set; }

        public List<OrderDetailsViewModel> OrderDetails { get; set; }
    }
}