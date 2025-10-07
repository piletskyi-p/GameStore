using System.Collections.Generic;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Enum;

namespace GameStore.Bll.Observer
{
    public class Manager
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public bool IsBanned { get; set; }

        public virtual List<Role> Roles { get; set; }

        public int SenderTypeId
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