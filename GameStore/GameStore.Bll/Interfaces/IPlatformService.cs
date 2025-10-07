using System.Collections.Generic;
using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface IPlatformService
    {
        List<PlatformDTO> GetAll(string lang);
        IEnumerable<PlatformDTO> GetByGameKey(string key, string lang);
        PlatformDTO GetById(int id, string lang);
        void Create(PlatformDTO platformDto);
        void Update(PlatformDTO platformDto);
        void Delete(int id);
    }
}
