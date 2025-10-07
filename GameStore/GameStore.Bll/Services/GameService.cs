using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Enums;
using GameStore.Bll.Filters;
using GameStore.Bll.Interfaces;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Entities;
using GameStore.Dal.Entities.Translate;
using GameStore.Dal.Filter;
using GameStore.Dal.Interfaces;
using NLog;

namespace GameStore.Bll.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventLogger _eventLogger;
        private readonly ILogger _logger;

        public GameService(ILogger logger, IUnitOfWork unitOfWork, IEventLogger eventLogger)
        {
            _eventLogger = eventLogger;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void Create(GameDTO gameDto)
        {
            var image = Mapper.Map<Image>(gameDto.Image);
            _unitOfWork.ImageRepository.Create(image);
            _unitOfWork.Save();

            var game = Mapper.Map<Game>(gameDto);
            foreach (var translate in game.GameTranslates)
            {
                translate.Language = _unitOfWork.LanguageRepository.FindById(translate.LanguageId);
            }

            game.Genres = gameDto.GenresIds.Select(id => _unitOfWork
                .GenreRepository.FindById(id)).ToList();

            game.Platforms = gameDto.PlatformsIds.Select(id => _unitOfWork
                .PlatformRepository.FindById(id)).ToList();

            game.Comments = new List<Comment>();
            game.ImageId = _unitOfWork.ImageRepository
                .Get(im => im.GameKey == game.Key).FirstOrDefault()
                ?.Id;

            game.UploadDate = DateTime.UtcNow;
            _unitOfWork.GameRepository.Create(game);
            _unitOfWork.Save();
            _eventLogger.LogCreate(Mapper.Map<Game>(game));

            _logger.Debug("(Class: GameService) The method \"Create(GameDTO item)\" worked.");
        }

        public void Delete(int id)
        {
            var game = _unitOfWork.GameRepository.Get(gameRepository => gameRepository.Id == id)
                .FirstOrDefault();

            if (game != null)
            {
                _unitOfWork.GameRepository.Delete(game.Id);
                _unitOfWork.Save();
                _eventLogger.LogDelete(game);

                _logger.Debug("(Class: GameService) The method \"Delete(int id)\" worked.");
            }
        }

        public void Edit(GameDTO gameDto)
        {
            if (gameDto.Image != null)
            {
                var image = _unitOfWork.ImageRepository
                    .Get(im => im.GameKey == gameDto.Key).FirstOrDefault();
                if (image != null)
                {
                    gameDto.Image.Id = image.Id;
                    Mapper.Map(gameDto.Image, image);
                    _unitOfWork.ImageRepository.Update(image);
                }

                _unitOfWork.Save();
            }

            var game = _unitOfWork.GameRepository.FindById(gameDto.Id);
            var gameOld = Mapper.Map<Game>(game);
            gameDto.UploadDate = game.UploadDate;
            Mapper.Map(gameDto, game);

            game.UploadDate = gameOld.UploadDate;
            game.Genres = gameDto.GenresIds.Select(id => _unitOfWork
                .GenreRepository.FindById(id,
                    repository => repository.Games,
                    repository => repository.GenreTranslates,
                    repository => repository.GenreTranslates
                        .Select(translate => translate.Language))).ToList();

            game.Platforms = gameDto.PlatformsIds.Select(id => _unitOfWork
                .PlatformRepository.FindById(id,
                    repository => repository.Games,
                    repository => repository.PlatformTranslates,
                    repository => repository.PlatformTranslates
                        .Select(translate => translate.Language))).ToList();

            game.Publisher = _unitOfWork.PublisherRepository.FindById(game.PublisherId,
                repository => repository.Games,
                repository => repository.PublisherTranslate,
                repository => repository.PublisherTranslate
                    .Select(translate => translate.Language));
            game.GameTranslates = Mapper.Map<ICollection<GameTranslate>>(gameDto.GameTranslates);
            foreach (var translate in game.GameTranslates)
            {
                translate.Language = _unitOfWork.LanguageRepository.FindById(translate.LanguageId);
            }
            game.ImageId = _unitOfWork.ImageRepository
                .Get(im => im.GameKey == game.Key).FirstOrDefault()
                ?.Id;

            _unitOfWork.GameRepository.Update(game);
            _unitOfWork.Save();
            _eventLogger.LogUpdate(Mapper.Map<Game>(gameOld), Mapper.Map<Game>(game));
            _logger.Debug("(Class: GameService) The method \"Create(GameDTO item)\" worked.");
        }

        public List<GameDTO> Filter(FilterDTO filtersDto, int page, int pageSize)
        {
            var gameSelectionPipeline = RegisterFilters(filtersDto);
            var games = _unitOfWork.GameRepository.Filter(gameSelectionPipeline, page, pageSize);
            var gameDto = Mapper.Map<List<GameDTO>>(games);
            foreach (var gamedto in gameDto)
            {
                gamedto.Image = Mapper.Map<ImageDto>(_unitOfWork.ImageRepository.FindById(gamedto.ImageId));
            }

            return gameDto;
        }

        public int FilterCount(FilterDTO filtersDto)
        {
            var gameSelectionPipeline = RegisterFilters(filtersDto);
            var count = _unitOfWork.GameRepository.FilterCount(gameSelectionPipeline);

            return count;
        }

        public IEnumerable<GameDTO> GetAllGames()
        {
            var game = _unitOfWork.GameRepository.Get().ToList();

            if (game.Any())
            {
                var gameDto = Mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(game);

                foreach (var gamedto in gameDto)
                {
                    gamedto.Image = Mapper.Map<ImageDto>(_unitOfWork.ImageRepository.FindById(gamedto.ImageId));
                }

                return gameDto;
            }

            return new List<GameDTO>();
        }

        public GameDTO GetGameByKey(string key, string lang)
        {
            var game = _unitOfWork.GameRepository
                .Get(games => games.Key == key,
                    repository => repository.Publisher,
                    repository => repository.GameTranslates,
                    repository => repository.GameTranslates.Select(tr => tr.Language),
                    repository => repository.Genres,
                    repository => repository.Genres.Select(genre => genre.GenreTranslates),
                    repository => repository.Platforms,
                    repository => repository.Platforms.Select(platf => platf.PlatformTranslates))
                .FirstOrDefault();

            if (game != null)
            {
                game.PopularityCounter++;
                _unitOfWork.GameRepository.Update(game);
                _unitOfWork.Save();

                game.Publisher = _unitOfWork.PublisherRepository
                    .FindById(game.PublisherId);

                _logger.Debug("(Class: GameService) The method \"GetGameByKey" +
                             "(string key)\" worked.");
                var gameDto = Mapper.Map<Game, GameDTO>(game);

                return SetUpFields(gameDto, lang);
            }

            _logger.Debug($"(Class: GameService) The method \"GetGameByKey(string key)\" worked.");

            return null;
        }

        public IEnumerable<GameDTO> GetGamesByGenre(string genre)
        {
            var games = _unitOfWork.GenreRepository
                .Get(genreRepository => genreRepository.GenreTranslates.Any(trans => trans.Name == genre)).ToList();

            if (games.Any())
            {
                var game = games.First().Games.ToList();
                var gameDto = Mapper.Map<List<Game>, List<GameDTO>>(game);

                _logger.Debug("(Class: GameService) The method \"GetGamesByGenre(string genre)\" worked.");

                return gameDto;
            }

            return null;
        }

        public IEnumerable<GameDTO> GetGamesByPlatform(string platform)
        {
            var type = _unitOfWork.PlatformRepository.Get(platformRepository => platformRepository
                         .PlatformTranslates.Any(trans => trans.Type == platform))
                .FirstOrDefault();

            if (type != null)
            {
                var game = type.Games;
                var gameDto = Mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(game);
                _logger.Debug("(Class: GameService) The method \"GetGamesByPlatform(string platform)\" worked.");

                return gameDto;
            }

            return null;
        }

        public IEnumerable<GameDTO> GetGamesByRange(int page, int pageSize)
        {
            var games = _unitOfWork.GameRepository.GetByRange(page, pageSize);
            var gameDto = Mapper.Map<IEnumerable<GameDTO>>(games);

            return gameDto;
        }

        public GameDTO GetItemById(int id, string lang)
        {
            var game = _unitOfWork.GameRepository
                .GetFromAll(gameRepository => gameRepository.Id == id,
                    repository => repository.Publisher,
                    repository => repository.GameTranslates,
                    repository => repository.GameTranslates.Select(tr => tr.Language),
                    repository => repository.Genres,
                    repository => repository.Genres.Select(genre => genre.GenreTranslates),
                    repository => repository.Platforms,
                    repository => repository.Platforms.Select(platf => platf.PlatformTranslates))
                .FirstOrDefault();
            if (game != null)
            {
                game.PopularityCounter++;
                _unitOfWork.GameRepository.Update(game);
                _unitOfWork.Save();

                game.Publisher = _unitOfWork.PublisherRepository
                    .FindById(game.PublisherId);
                _logger.Debug("(Class: GameService) The method \"GetGameByKey" +
                              "(string key)\" worked.");
                var gameDto = Mapper.Map<Game, GameDTO>(game);

                return SetUpFields(gameDto, lang);
            }

            return null;
        }

        public void UpdateForRemove(int id)
        {
            var gameForRemove = _unitOfWork.GameRepository
                .Get(game => game.Id == id).FirstOrDefault();

            if (gameForRemove != null)
            {
                gameForRemove.IsDeleted = true;
                _unitOfWork.GameRepository.Update(gameForRemove);
                var orders = _unitOfWork.OrdersRepository
                    .GetFromSql(order => order.IsPaid == false,
                        repository => repository.OrderDetails).ToList();

                if (orders.Any())
                {
                    foreach (var order in orders)
                    {
                        if (order.OrderDetails.Count != 0)
                        {
                            var del = order.OrderDetails
                                .FirstOrDefault(det => det.ProductID == gameForRemove.Key);
                            if (del != null)
                            {
                                _unitOfWork.OrderDetailsRepository.HardDelete(del);
                            }
                        }
                    }
                }

                _unitOfWork.Save();
                _eventLogger.LogDelete(Mapper.Map<Game>(gameForRemove));
                _logger.Debug("(Class: GameService) The method \"Update(GameDTO item)\" worked.");
            }
        }

        public GameSelectionPipeline RegisterFilters(FilterDTO filters)
        {
            var gameSelectionPipeline = new GameSelectionPipeline();

            if (filters.GenresId != null && filters.GenresId.Any())
            {
                gameSelectionPipeline.Register(new FilterByGenre(filters.GenresId));
            }

            if (filters.PublisherDateId != 0)
            {
                filters.PublisherDateEnum = (DateEnum)filters.PublisherDateId - 1;
                gameSelectionPipeline.Register(new FilterByDate(filters.PublisherDateEnum));
            }

            if (!string.IsNullOrEmpty(filters.SearchName))
            {
                gameSelectionPipeline.Register(new FilterByName(filters.SearchName));
            }

            if (filters.PlatformsId != null && filters.PlatformsId.Any())
            {
                gameSelectionPipeline.Register(new FilterByPlatform(filters.PlatformsId));
            }

            if (filters.PriceRangeFrom != 0 || filters.PriceRangeTo != 0)
            {
                gameSelectionPipeline.Register(new FilterByPrice(filters.PriceRangeFrom, filters.PriceRangeTo));
            }

            if (filters.PublishersId != null && filters.PublishersId.Any())
            {
                gameSelectionPipeline.Register(new FilterByPublisher(filters.PublishersId));
            }

            if (filters.SortBy != 0)
            {
                gameSelectionPipeline.Register(new FilterSort(filters.SortBy));
            }

            return gameSelectionPipeline;
        }

        public IEnumerable<GameDTO> GetDeletedGames()
        {
            var games = _unitOfWork.GameRepository.GetDeleted();
            var gamesDto = Mapper.Map<IEnumerable<GameDTO>>(games);

            return gamesDto;
        }

        public GameDTO GetDeletedGameDetails(string key, string lang)
        {
            var game = _unitOfWork.GameRepository.GetDeleted(games => games.Key == key,
                        repository => repository.Publisher,
                        repository => repository.GameTranslates,
                        repository => repository.GameTranslates.Select(tr => tr.Language),
                        repository => repository.Genres,
                        repository => repository.Genres.Select(genre => genre.GenreTranslates),
                        repository => repository.Platforms,
                        repository => repository.Platforms.Select(platf => platf.PlatformTranslates))
                    .FirstOrDefault();

            if (game != null)
            {
                game.Publisher = _unitOfWork.PublisherRepository
                    .FindById(game.PublisherId);
                _logger.Debug("(Class: GameService) The method \"GetGameByKey" +
                              "(string key)\" worked.");
                var gameDto = Mapper.Map<Game, GameDTO>(game);

                return SetUpFields(gameDto, lang);
            }

            _logger.Debug($"(Class: GameService) The method \"GetGameByKey(string key)\" worked.");

            return null;
        }

        public GameDTO GetDeletedGameDetailsById(int id, string lang)
        {
            var game = _unitOfWork.GameRepository.GetDeleted(games => games.Id == id,
                    repository => repository.Publisher,
                    repository => repository.GameTranslates,
                    repository => repository.GameTranslates.Select(tr => tr.Language),
                    repository => repository.Genres,
                    repository => repository.Genres.Select(genre => genre.GenreTranslates),
                    repository => repository.Platforms,
                    repository => repository.Platforms.Select(platf => platf.PlatformTranslates))
                .FirstOrDefault();

            if (game != null)
            {
                game.Publisher = _unitOfWork.PublisherRepository
                    .FindById(game.PublisherId);
                _logger.Debug("(Class: GameService) The method \"GetGameByKey" +
                              "(string key)\" worked.");
                var gameDto = Mapper.Map<Game, GameDTO>(game);

                return SetUpFields(gameDto, lang);
            }

            _logger.Debug($"(Class: GameService) The method \"GetGameByKey(string key)\" worked.");

            return null;
        }

        private GameDTO SetUpFields(GameDTO game, string lang)
        {
            foreach (var genre in game.Genres)
            {
                genre.Name = genre
                    .GenreTranslates.First(tr => tr.Language.Key == lang).Name;
            }

            foreach (var platform in game.Platforms)
            {
                platform.Type = platform
                    .PlatformTranslates.First(tr => tr.Language.Key == lang).Type;
            }

            game.Publisher.Description = game.Publisher
                .PublisherTranslate.FirstOrDefault(tr => tr.Language.Key == lang)?.Description;

            game.Description = game.GameTranslates
                .FirstOrDefault(tr => tr.Language.Key == lang)?.Description;

            game.Image = Mapper.Map<ImageDto>(_unitOfWork.ImageRepository
                .Get(image => image.Id == game.ImageId).FirstOrDefault());

            if (game.ImageId != 0)
            {
                game.Image = Mapper.Map<ImageDto>(_unitOfWork.ImageRepository
                    .Get(image => image.Id == game.ImageId).FirstOrDefault());
            }

            return game;
        }

        public async Task<ImageDto> AsyncGetImageByKey(string key)
        {
            var image = _unitOfWork.ImageRepository.Get(im => im.GameKey == key).FirstOrDefault();

            if (image != null)
            {
                var imageDto = Mapper.Map<ImageDto>(image);

                return imageDto;
            }

            return null;
        }

        public ImageDto GetImageByKey(string key)
        {
            var image = _unitOfWork.ImageRepository.Get(im => im.GameKey == key).FirstOrDefault();

            if (image != null)
            {
                var imageDto = Mapper.Map<ImageDto>(image);

                return imageDto;
            }

            return null;
        }

        public IEnumerable<GameDTO> GetGamesByGenreId(int id)
        {
            var genre = _unitOfWork.GenreRepository.FindById(id);
            var games = Mapper.Map<IEnumerable<GameDTO>>(genre.Games);

            return games;
        }

        public void SetRating(string key, double mark, string userName)
        {
            var game = _unitOfWork.GameRepository.Get(i => i.Key == key).FirstOrDefault();
            User user = null;

            if (userName != "anonym")
            {
                user = _unitOfWork.UserRepository.Get(i => i.Email == userName,
                    repository => repository.Rates,
                    repository => repository.Roles).FirstOrDefault();
            }

            if (game != null)
            {
                game.RatingMarks += $",{mark}";
                game.Rating = GetMedian(game.RatingMarks);
                _unitOfWork.GameRepository.Update(game);

                if (user != null)
                {
                    var ratesForUser = user.Rates.FirstOrDefault(i => i.Game.Key == key);

                    if (ratesForUser == null)
                    {
                        user.Rates.Add(new Rate
                        {
                            Game = game,
                            RatingMarks = $"{mark}",
                            Rating = mark
                        });
                    }
                    else
                    {
                        ratesForUser.RatingMarks += $",{mark}";
                        ratesForUser.Rating = GetMedian(ratesForUser.RatingMarks);
                    }

                    _unitOfWork.UserRepository.Update(user);
                }

                _unitOfWork.Save();
            }
        }

        private double GetMedian(string marksString)
        {
            var markList = new List<double>();
            var marks = marksString.Split(',');
            foreach (var mark in marks)
            {
                if (double.TryParse(mark, out var temp))
                {
                    markList.Add(temp);
                }
            }

            double[] sortedPNumbers = (double[])markList.ToArray().Clone();
            Array.Sort(sortedPNumbers);

            int size = sortedPNumbers.Length;
            int mid = size / 2;
            double median = (size % 2 != 0) ? sortedPNumbers[mid] : (sortedPNumbers[mid] + sortedPNumbers[mid - 1]) / 2;

            return median;
        }
    }
}