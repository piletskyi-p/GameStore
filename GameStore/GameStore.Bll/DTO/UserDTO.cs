using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Dal.Entities.Entities;
using GameStore.Dal.Entities.Enum;

namespace GameStore.Bll.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsPersistent { get; set; }

        public ICollection<RoleDTO> Roles { get; set; }

        public ICollection<RateDTO> Rates { get; set; }

        public PublisherDTO Publisher { get; set; }

        public DateTime BannedUntil { get; set; }

        public int PublisherId { get; set; }

        public IEnumerable<int> RoleIds { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBanned { get; set; }

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


        public bool InRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
            {
                return false;
            }

            var rolesArray = roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in rolesArray)
            {
                var hasRole = Roles.Where(r => r.Name == role);
                if (hasRole.Any())
                {
                    return true;
                }
            }

            return false;
        }

    }
}
