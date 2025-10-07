using GameStore.Web.Pagination;

namespace GameStore.Web.Models.ViewModels
{
    public class FilterIndexViewModel
    {
        public FilterViewModel FilterModel { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}