using System.Collections.Generic;
using GameStore.Dal.Entities.Translate;

namespace GameStore.Dal.Entities
{
    public class Genre : BaseEntity
    {
        public Genre()
        {
        }

        public int? ParentId { get; set; }

        public virtual Genre Parent { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        public virtual ICollection<GenreTranslate> GenreTranslates { get; set; }
    }
}
