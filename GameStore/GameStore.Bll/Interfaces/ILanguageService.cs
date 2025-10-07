using System.Collections.Generic;
using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface ILanguageService
    {
        IEnumerable<LanguageDto> GetAll();
        LanguageDto GetById(int id);
        LanguageDto GetByKey(string key);
    }
}