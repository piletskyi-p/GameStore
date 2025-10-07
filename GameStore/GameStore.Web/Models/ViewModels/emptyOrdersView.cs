using System;
using System.Collections.Generic;
using GameStore.Web.Pagination;

namespace GameStore.Web.Models.ViewModels
{
    public class EmptyOrdersView
    {
        public List<OrderViewModel> Orders { get; set; }
        public PageInfo PageInfo { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}