using System;
using System.Collections.Generic;
using GameStore.Dal.Entities.Translate;

namespace GameStore.Dal.Entities
{
    public class Game : BaseEntity
    {
        public Game() { }

        public string Key { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int UnitInStock { get; set; }

        public bool IsDiscontinued { get; set; }

        public int PublisherId { get; set; }

        public int PopularityCounter { get; set; }

        public double Rating { get; set; }

        public string RatingMarks { get; set; }

        public int? ImageId { get; set; }

        public DateTime UploadDate { get; set; }

        public DateTime PublicationDate { get; set; }

        public Publisher Publisher { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }

        public virtual ICollection<Platform> Platforms { get; set; }

        public virtual ICollection<GameTranslate> GameTranslates { get; set; }
    }
}
