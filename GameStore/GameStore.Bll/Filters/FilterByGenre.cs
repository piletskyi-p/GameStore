using System.Collections.Generic;
using System.Linq;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Filters
{
    public class FilterByGenre : IFilter<IQueryable<Game>>
    {
        private readonly List<int> _genresId;

        public FilterByGenre(List<int> genresId)
        {
            _genresId = genresId;
        }

        public IQueryable<Game> Execute(IQueryable<Game> games)
        {
            if (_genresId.Any())
            {
                return games.Where(game => game.Genres
                    .Any(genre => _genresId.Contains(genre.Id)));
            }

            return games;
        }
    }
}
