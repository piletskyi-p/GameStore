using System.Collections.Generic;
using GameStore.Dal.Entities.Translate;

namespace GameStore.Dal.Entities
{
    public class Platform : BaseEntity
    {
        public Platform()
        {
        }

        public virtual ICollection<Game> Games { get; set; }

        public virtual ICollection<PlatformTranslate> PlatformTranslates { get; set; }
    }
}
