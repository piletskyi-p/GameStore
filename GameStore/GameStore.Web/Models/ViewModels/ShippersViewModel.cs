using System;

namespace GameStore.Web.Models.ViewModels
{
    public class ShippersViewModel
    {
        public Object _id { get; set; }

        public int ShipperID { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }
    }
}