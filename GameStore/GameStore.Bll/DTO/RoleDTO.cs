using GameStore.Dal.Entities;
using System.Collections.Generic;

namespace GameStore.Bll.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}