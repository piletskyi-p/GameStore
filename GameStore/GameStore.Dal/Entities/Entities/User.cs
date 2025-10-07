using System;
using System.Collections.Generic;
using GameStore.Dal.Entities.Entities;
using GameStore.Dal.Entities.Enum;

namespace GameStore.Dal.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsPersistent { get; set; }

        public bool IsBanned { get; set; }

        public virtual Publisher Publisher { get; set; }

        public DateTime BannedUntil { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }

        public virtual int SenderTypeId
        {
            get
            {
                return (int)this.SenderType;
            }
            set
            {
                SenderType = (Sender)value;
            }
        }

        public Sender SenderType { get; set; }
    }
}
