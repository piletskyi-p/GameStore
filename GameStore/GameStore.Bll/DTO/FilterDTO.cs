using System.Collections.Generic;
using GameStore.Bll.Enums;

namespace GameStore.Bll.DTO
{
    public class FilterDTO
    {
        public List<GenreDTO> Genres { get; set; }

        public List<PlatformDTO> Platforms { get; set; }

        public List<PublisherDTO> Publishers { get; set; }

        public List<int> GenresId { get; set; }

        public List<int> PlatformsId { get; set; }

        public List<int> PublishersId { get; set; }

        public List<GameDTO> Games { get; set; }

        public List<string> GameNames { get; set; }

        public int SortBy { get; set; }

        public decimal PriceRangeFrom { get; set; }

        public decimal PriceRangeTo { get; set; }

        public int PublisherDateId { get; set; }

        public DateEnum PublisherDateEnum { get; set; }

        public string SearchName { get; set; }

        public int NumberOfItemPage { get; set; }
    }
}
