using System;

namespace GameStore.Dal.Entities
{
    public class UserToken : BaseEntity
    {
        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime DropDateTime { get; set; }
    }
}