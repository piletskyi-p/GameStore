using System;
using System.Collections.Generic;
using GameStore.Web.Models.LanguageModels;

namespace GameStore.Web.Models.ViewModels
{
    public class GameDetailsViewModel
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public decimal Price { get; set; }

        public int UnitInStock { get; set; }

        public DateTime UploadDate { get; set; }

        public DateTime PublicationDate { get; set; }

        public PublisherViewModel Publisher { get; set; }

        public List<GenreViewModel> Genres { get; set; }

        public List<PlatformViewModel> Platforms { get; set; }

        public List<GenreTranslateModel> GameTranslates { get; set; }
    }
}