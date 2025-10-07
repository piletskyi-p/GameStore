﻿using System.Collections.Generic;

namespace GameStore.Dal.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}