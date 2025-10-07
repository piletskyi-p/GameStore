using System.Collections.Generic;
using System.Linq;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Filters
{
    public class FilterByPlatform : IFilter<IQueryable<Game>>
    {
        private readonly List<int> _platformsId;

        public FilterByPlatform(List<int> platformsId)
        {
            _platformsId = platformsId;
        }

        public IQueryable<Game> Execute(IQueryable<Game> games)
        {
            if (_platformsId.Any())
            {
                return games.Where(game => game.Platforms
                    .Any(platform => _platformsId.Contains(platform.Id)));
            }

            return games;
        }
    }
}
