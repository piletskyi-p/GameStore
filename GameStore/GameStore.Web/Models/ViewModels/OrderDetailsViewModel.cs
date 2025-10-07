using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        public string ProductID { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public double Discount { get; set; }

        [Required]
        public int OrderId { get; set; }
    }
}