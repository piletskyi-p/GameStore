using System.Collections.Generic;
using GameStore.Web.Pagination;

namespace GameStore.Web.Models.ViewModels
{
    public class IndexGameViewModel
    {
        public List<GameViewModel> Games { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}