using System;
using System.Linq;
using GameStore.Bll.Enums;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Filters
{
    public class FilterByDate : IFilter<IQueryable<Game>>
    {
        private readonly DateEnum _time;

        public FilterByDate(DateEnum publisherDate)
        {
            _time = publisherDate;
        }

        public IQueryable<Game> Execute(IQueryable<Game> games)
        {
            DateTime date;
            
            switch (_time)
            {
                case DateEnum.LastWeek:
                    date = DateTime.UtcNow.AddDays(-7);
                    return games.Where(game => game
                    .PublicationDate > date);
                case DateEnum.LastMonth:
                    date = DateTime.UtcNow.AddMonths(-1);
                    return games.Where(game => game
                    .PublicationDate > date);
                case DateEnum.LastYear:
                    date = DateTime.UtcNow.AddYears(-1);
                    return games.Where(game => game
                    .PublicationDate > date);
                case DateEnum.TwoYearsAgo:
                    date = DateTime.UtcNow.AddYears(-2);
                    return games.Where(game => game
                    .PublicationDate > date);
                case DateEnum.ThreeYearsAgo:
                    date = DateTime.UtcNow.AddYears(-3);
                    return games.Where(game => game
                    .PublicationDate > date);
                default:
                    return games;
            }
        }
    }
}