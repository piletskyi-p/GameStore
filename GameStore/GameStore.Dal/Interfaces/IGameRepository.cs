using System.Collections.Generic;
using GameStore.Dal.Entities;
using GameStore.Dal.Filter;

namespace GameStore.Dal.Interfaces
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        int FilterCount(GameSelectionPipeline gameSelection);
        IEnumerable<Game> Filter(GameSelectionPipeline gameSelection, int page, int pageSize);
        //Game GameDetailsById(int id);
        //Game GameDetailsByKey(string key);
        //IEnumerable<Game> GetAll();
    }
}
