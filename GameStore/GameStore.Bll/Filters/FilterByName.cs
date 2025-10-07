using System.Linq;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Filters
{
    public class FilterByName : IFilter<IQueryable<Game>>
    {
        private readonly string _name;

        public FilterByName(string name)
        {
            _name = name;
        }

        public IQueryable<Game> Execute(IQueryable<Game> games)
        {
            if (!string.IsNullOrEmpty(_name))
            {
                return games
                    .Where(game => game.Name.ToUpper().Contains(_name.ToUpper()));
            }

            return games;
        }
    }
}
