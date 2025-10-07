using System.Linq;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Filters
{
    public class FilterByPrice : IFilter<IQueryable<Game>>
    {
        private readonly decimal _priceFrom;
        private readonly decimal _priceTo;

        public FilterByPrice(decimal from, decimal to)
        {
            _priceFrom = from;
            _priceTo = to;
        }

        public IQueryable<Game> Execute(IQueryable<Game> games)
        {
            if (_priceFrom == 0 && _priceTo == 0)
            {
                return games;
            }

            if (_priceFrom != 0 && _priceTo == 0)
            {
                return games.Where(game => game.Price > _priceFrom);
            }

            if (_priceFrom == 0 && _priceTo != 0)
            {
                return games.Where(game => game.Price < _priceTo);
            }

            return games.Where(game => game.Price > _priceFrom &&
                                       game.Price < _priceTo);
        }
    }
}
