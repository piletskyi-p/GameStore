using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface IGameService
    {
        void Create(GameDTO item);
        void UpdateForRemove(int id);
        void Delete(int id);
        void Edit(GameDTO gameDto);

        GameDTO GetItemById(int id, string lang);
        GameDTO GetGameByKey(string key, string lang);

        IEnumerable<GameDTO> GetGamesByGenre(string genre);
        IEnumerable<GameDTO> GetGamesByPlatform(string platform);
        IEnumerable<GameDTO> GetAllGames();
        IEnumerable<GameDTO> GetGamesByRange(int page, int pageSize);
        IEnumerable<GameDTO> GetDeletedGames();
        IEnumerable<GameDTO> GetGamesByGenreId(int id);

        GameDTO GetDeletedGameDetails(string key, string lang);
        GameDTO GetDeletedGameDetailsById(int id, string lang);

        int FilterCount(FilterDTO filters);
        List<GameDTO> Filter(FilterDTO filters, int page, int pageSize);
        // List<GameDTO> Filter(FilterDTO filters);

        Task<ImageDto> AsyncGetImageByKey(string key);
        ImageDto GetImageByKey(string key);

        void SetRating(string key, double mark, string userName);
    }
}
