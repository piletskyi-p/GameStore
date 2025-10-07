using System.Collections.Generic;
using GameStore.Bll.Enums;

namespace GameStore.Web.Models.ViewModels
{
    public class FilterViewModel
    {
        public List<GenreViewModel> Genres { get; set; }

        public List<PlatformViewModel> Platforms { get; set; }

        public List<PublisherViewModel> Publishers { get; set; }

        public List<int> GenresId { get; set; }

        public List<int> PlatformsId { get; set; }

        public List<int> PublishersId { get; set; }

        public List<GameViewModel> Games { get; set; }

        public List<string> GameNames { get; set; }

        public string SearchName { get; set; }
        
        public int PublisherDateId { get; set; }

        public DateEnum PublisherDateEnum { get; set; }

        public int NumberOfItemPage { get; set; }

        public int SortBy { get; set; }

        public decimal PriceRangeFrom { get; set; }

        public decimal PriceRangeTo { get; set; }
    }
}