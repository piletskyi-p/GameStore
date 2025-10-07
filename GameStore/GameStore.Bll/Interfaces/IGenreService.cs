using System.Collections.Generic;
using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface IGenreService
    {
        IEnumerable<GenreDTO> GetAll(string lang);
        IEnumerable<GenreDTO> GetByGameKey(string key, string lang);
        GenreDTO GetById(int id, string lang);
        void Create(GenreDTO genreDto);
        void Update(GenreDTO genreDto);
        void Delete(int id);
    }
}