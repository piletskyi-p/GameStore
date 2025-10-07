using System.Collections.Generic;
using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<RoleDTO> GetAll();
        void AddRole(RoleDTO roleDto);
    }
}