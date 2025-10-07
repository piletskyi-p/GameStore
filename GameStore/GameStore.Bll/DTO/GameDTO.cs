using System;
using System.Collections.Generic;
using GameStore.Bll.DTO.TranslateDto;

namespace GameStore.Bll.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<GameTranslateDto> GameTranslates { get; set; }

        public decimal Price { get; set; }

        public int UnitInStock { get; set; }

        public int PublisherId { get; set; }

        public bool IsDeleted { get; set; }

        public double Rating { get; set; }

        public int PopularityCounter { get; set; }

        public int ImageId { get; set; }

        public ImageDto Image { get; set; }

        public DateTime UploadDate { get; set; }

        public DateTime PublicationDate { get; set; }

        public PublisherDTO Publisher { get; set; }

        public bool IsDiscontinued { get; set; }

        public List<CommentDTO> Comments { get; set; }

        public List<int> GenresIds { get; set; }

        public List<GenreDTO> Genres { get; set; }

        public List<int> PlatformsIds { get; set; }

        public List<PlatformDTO> Platforms { get; set; }
    }
}
