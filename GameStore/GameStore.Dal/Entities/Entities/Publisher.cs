using System.Collections.Generic;
using GameStore.Dal.Entities.Translate;

namespace GameStore.Dal.Entities
{
    public class Publisher : BaseEntity
    {
        public Publisher()
        {
        }

        public string CompanyName { get; set; }

        public string HomePage { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<PublisherTranslate> PublisherTranslate { get; set; }
    }
}