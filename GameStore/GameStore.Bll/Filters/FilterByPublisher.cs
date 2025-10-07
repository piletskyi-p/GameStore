using System.Collections.Generic;
using System.Linq;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Filters
{
    public class FilterByPublisher : IFilter<IQueryable<Game>>
    {
        private readonly List<int> _publishersId;

        public FilterByPublisher(List<int> publishersId)
        {
            _publishersId = publishersId;
        }

        public IQueryable<Game> Execute(IQueryable<Game> games)
        {
            if (_publishersId.Any())
            {
                return games.Where(game => _publishersId.Contains(game.PublisherId));
            }
             
            return games;
        }
    }
}
