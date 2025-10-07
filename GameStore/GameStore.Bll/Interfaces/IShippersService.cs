using System.Collections.Generic;
using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface IShippersService
    {
        IEnumerable<ShippersDTO> Get();
    }
}