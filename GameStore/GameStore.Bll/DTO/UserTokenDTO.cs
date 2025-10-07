using System;

namespace GameStore.Bll.DTO
{
    public class UserTokenDto
    {
        public int Id { get; set; }

        public int IsDeleted { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime DropDateTime { get; set; }
    }
}