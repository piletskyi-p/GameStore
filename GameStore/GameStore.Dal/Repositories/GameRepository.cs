using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GameStore.Dal.Entities;
using GameStore.Dal.Filter;
using GameStore.Dal.Interfaces;

namespace GameStore.Dal.Repositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        private readonly DbSet<Game> _setDb;
        private readonly IDataBaseConnection _context;

        public GameRepository(IDataBaseConnection context) : base(context)
        {
            _setDb = context.Set<Game>();
            _context = context;
        }

        public IEnumerable<Game> Filter(GameSelectionPipeline gameSelection, int page, int pageSize)
        {
            var result = gameSelection.Process(_setDb);

            return result.ToList().Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int FilterCount(GameSelectionPipeline gameSelection)
        {
            var result = gameSelection.Process(_setDb);

            return result.ToList().Count();
        }

        //public Game GameDetailsById(int id)
        //{
        //    var game = _setDb.Where(g => g.Id == id).Include(genres => genres.Genres)
        //        .Include(genres => genres.Genres.Select(genre => genre.GenreTranslates))
        //        .Include(platforms => platforms.Platforms)
        //        .Include(platforms => platforms.Platforms.Select(platform => platform.PlatformTranslates))
        //        .Include(publisher => publisher.Publisher)
        //        .Include(publisher => publisher.Publisher.PublisherTranslate)
        //        .Include(g => g.GameTranslates).ToList();

        //    return game.FirstOrDefault();
        //}

        //public Game GameDetailsByKey(string key)
        //{
        //    var game = _setDb.Where(g => g.Key == key).Include(genres => genres.Genres)
        //        .Include(genres => genres.Genres.Select(genre => genre.GenreTranslates))
        //        .Include(platforms => platforms.Platforms)
        //        .Include(platforms => platforms.Platforms.Select(platform => platform.PlatformTranslates))
        //        .Include(publisher => publisher.Publisher)
        //        .Include(publisher => publisher.Publisher.PublisherTranslate)
        //        .Include(g => g.GameTranslates).FirstOrDefault();

        //    return game;
        //}

        //public IEnumerable<Game> GetAll()
        //{
        //    var games = _setDb.Where(g => !g.IsDeleted).Include(genres => genres.Genres)
        //        .Include(genres => genres.Genres.Select(genre => genre.GenreTranslates))
        //        .Include(platforms => platforms.Platforms)
        //        .Include(platforms => platforms.Platforms.Select(platform => platform.PlatformTranslates))
        //        .Include(publisher => publisher.Publisher)
        //        .Include(publisher => publisher.Publisher.PublisherTranslate)
        //        .Include(g => g.GameTranslates).ToList();

        //    return games;
        //}
    }
}