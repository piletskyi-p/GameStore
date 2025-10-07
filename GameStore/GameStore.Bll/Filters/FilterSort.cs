using System.Linq;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Filters
{
    public class FilterSort : IFilter<IQueryable<Game>>
    {
        private readonly int _sortBy;

        public FilterSort(int sort)
        {
            _sortBy = sort;
        }

        public IQueryable<Game> Execute(IQueryable<Game> games)
        {
            switch (_sortBy)
            {
                case 1:
                    return games.OrderBy(game => game.PopularityCounter);
                case 2:
                    return games.OrderBy(game => game.Comments.Count);
                case 3:
                    return games.OrderBy(game => game.Price);
                case 4:
                    return games.OrderByDescending(game => game.Price);
                case 5:
                    return games.OrderByDescending(game => game.UploadDate);
                default:
                    return games;
            }
        }
    }
}