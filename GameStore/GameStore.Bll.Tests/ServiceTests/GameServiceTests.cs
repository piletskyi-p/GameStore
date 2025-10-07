using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.DTO.TranslateDto;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Interfaces;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Translate;
using GameStore.Dal.Filter;
using GameStore.Dal.Interfaces;
using GameStore.Web;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServiceTests
{
    [TestFixture]
    public class GameServiceTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IEventLogger> _baseService;
        private GameService _gameService;
        private Mock<IUnitOfWork> _unitOfWork;

        public GameServiceTests()
        {
            _logger = new Mock<ILogger>();
            _mapper = new Mock<IMapper>();
            _baseService = new Mock<IEventLogger>();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _gameService = new GameService(_logger.Object, _unitOfWork.Object, _baseService.Object);
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutomapperWebProfile>();
                cfg.AddProfile<MapProfile>();
            });
        }

        [Test]
        public void GetAllGames_GamesGetsSuccesfully_ReturnsGames()
        {
            var game = new List<Game>
            {
                new Game(),
                new Game()
            };
            IEnumerable<GameDTO> resultList = new List<GameDTO>
            {
                new GameDTO(),
                new GameDTO()
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Get()).Returns(game);
            _unitOfWork.Setup(unit => unit.ImageRepository
                .FindById(It.IsAny<int>())).Returns(new Image());
            _mapper.Setup(mapper => mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(
                It.IsAny<IEnumerable<Game>>())).Returns(resultList);

            resultList = _gameService.GetAllGames();
            Assert.IsNotEmpty(resultList);
        }

        [Test]
        public void GetAllGames_GamesGetsTheSameSize_ReturnsGames()
        {
            var games = new List<Game>
            {
                new Game(),
                new Game()
            };
            IEnumerable<GameDTO> resultList = new List<GameDTO>
            {
                new GameDTO(),
                new GameDTO()
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Get()).Returns(games);
            _unitOfWork.Setup(unit => unit.ImageRepository
                .FindById(It.IsAny<int>())).Returns(new Image());
            _mapper.Setup(mapper => mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(
                It.IsAny<IEnumerable<Game>>())).Returns(resultList);

            var result = _gameService.GetAllGames();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetGameByKey_GetGameByKey_ReturnsGame()
        {
            string key = "ME";
            var localresult = new GameDTO
            {
                Id = 1,
                Key = key,
                Name = "Mass Effect",
                GameTranslates = new List<GameTranslateDto>(),
                Publisher = new PublisherDTO
                {
                    PublisherTranslate = new List<PublisherTranslateDto>()
                }
            };
            var game = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Key = key,
                    Name = "Mass Effect",
                    GameTranslates = new List<GameTranslate>(),
                    Publisher = new Publisher
                    {
                        PublisherTranslate = new List<PublisherTranslate>()
                    }
                }
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork
                    .GameRepository.Get(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(game);
            _unitOfWork.Setup(service => service.PublisherRepository
                .FindById(It.IsAny<int>())).Returns(new Publisher());
            _unitOfWork.Setup(unit => unit.GameRepository.Update(It.IsAny<Game>()));
            _unitOfWork.Setup(unit => unit.ImageRepository
                .Get(It.IsAny<Expression<Func<Image, bool>>>())).Returns(new List<Image>());
            _mapper.Setup(mapper => mapper.Map<Game, GameDTO>(
                It.IsAny<Game>())).Returns(localresult);

            var result = _gameService.GetGameByKey(key, "lang");

            Assert.AreEqual(key, result.Key);
        }

        [Test]
        public void GetGameByKey_GetNotNull_ReturnsGame()
        {
            string key = "ME";
            var localresult = new GameDTO
            {
                Id = 1,
                Key = key,
                Name = "Mass Effect",
                GameTranslates = new List<GameTranslateDto>(),
                Publisher = new PublisherDTO
                {
                    PublisherTranslate = new List<PublisherTranslateDto>()
                }
            };
            var game = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Key = key,
                    Name = "Mass Effect",
                    GameTranslates = new List<GameTranslate>(),
                    Publisher = new Publisher
                    {
                        PublisherTranslate = new List<PublisherTranslate>()
                    }
                }
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(game);
            _unitOfWork.Setup(service => service.PublisherRepository
                .FindById(It.IsAny<int>())).Returns(new Publisher());
            _unitOfWork.Setup(unit => unit.GameRepository.Update(It.IsAny<Game>()));
            _unitOfWork.Setup(unit => unit.ImageRepository
                .Get(It.IsAny<Expression<Func<Image, bool>>>())).Returns(new List<Image>());
            _mapper.Setup(mapper => mapper.Map<Game, GameDTO>(
                It.IsAny<Game>())).Returns(localresult);

            var result = _gameService.GetGameByKey(key, "lang");

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetGameByKey_GetNull_ReturnsGame()
        {
            var resultL = new GameDTO();
            string key = string.Empty;
            var game = new List<Game>();
            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(game);
            _mapper.Setup(mapper => mapper.Map<Game, GameDTO>(
                It.IsAny<Game>())).Returns(resultL);

            var result = _gameService.GetGameByKey(key, string.Empty);

            Assert.IsNull(result);
        }

        [Test]
        public void GetGameByGenre_GetNullIfGenreIsEmpty_ReturnsGames()
        {
            IEnumerable<GameDTO> resultList = new List<GameDTO>();
            string genre = string.Empty;
            var game = new List<Genre>();

            _unitOfWork.Setup(unitOfWork => unitOfWork.GenreRepository
                    .Get(It.IsAny<Expression<Func<Genre, bool>>>(),
                    It.IsAny<Expression<Func<Genre, object>>[]>()))
                .Returns(game);
            _mapper.Setup(mapper => mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(
                It.IsAny<IEnumerable<Game>>())).Returns(resultList);

            resultList = _gameService.GetGamesByGenre(genre);
            Assert.IsNull(resultList);
        }

        [Test]
        public void GetGameByGenre_GetGameIfGenreExists_ReturnsGames()
        {
            IEnumerable<GameDTO> resultList = new List<GameDTO>();
            string genre = "RPG";
            var game = new List<Genre>
            {
                new Genre
                {
                    GenreTranslates = new List<GenreTranslate>(),
                    Games = new List<Game> { new Game() }
                }
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork.GenreRepository
                    .Get(It.IsAny<Expression<Func<Genre, bool>>>(),
                    It.IsAny<Expression<Func<Genre, object>>[]>()))
                .Returns(game);
            _mapper.Setup(mapper => mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(
                It.IsAny<IEnumerable<Game>>())).Returns(resultList);

            resultList = _gameService.GetGamesByGenre(genre);
            Assert.IsNotEmpty(resultList);
        }

        [Test]
        public void GetItemById_GetGameWithNeededID_ReturnsGame()
        {
            string key = "ME";
            var localresult = new List<GameDTO>
            {
                new GameDTO
                {
                    Id = 1,
                    Key = key,
                    Name = "Mass Effect",
                    GameTranslates = new List<GameTranslateDto>(),
                    Publisher = new PublisherDTO
                    {
                        PublisherTranslate = new List<PublisherTranslateDto>()
                    }
                }
            };
            var game = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Key = key,
                    Name = "Mass Effect",
                    GameTranslates = new List<GameTranslate>(),
                    Publisher = new Publisher
                    {
                        PublisherTranslate = new List<PublisherTranslate>()
                    }
                }
            };

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .GetFromAll(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(game);
            _unitOfWork.Setup(service => service.PublisherRepository
                .FindById(It.IsAny<int>())).Returns(new Publisher());
            _unitOfWork.Setup(unit => unit.GameRepository.Update(It.IsAny<Game>()));
            _unitOfWork.Setup(unit => unit.ImageRepository
                .Get(It.IsAny<Expression<Func<Image, bool>>>())).Returns(new List<Image>());
            _mapper.Setup(mapper => mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(
                It.IsAny<IEnumerable<Game>>())).Returns(localresult);

            var result = _gameService.GetItemById(1, "lang");

            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void GetItemById_GetNullIfIdNotExist_ReturnsGame()
        {
            IEnumerable<GameDTO> resultList = new List<GameDTO>();
            var game = new List<Game>();

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(game);
            _mapper.Setup(mapper => mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(
                It.IsAny<IEnumerable<Game>>())).Returns(resultList);

            var result = _gameService.GetItemById(2, "lang");

            Assert.IsNull(result);
        }

        [Test]
        public void Create_GameRepositoryCreate_WorkMostOnce()
        {
            var game = new Game
            {
                Id = 1,
                Name = "qqq",
                Key = "qeew",
                GameTranslates = new List<GameTranslate>(),
                IsDeleted = false,
                Comments = new List<Comment>
                {
                    new Comment()
                },
                Genres = new List<Genre>
                {
                    new Genre()
                },
                Platforms = new List<Platform>
                {
                    new Platform()
                }
            };
            var gameDto = new GameDTO
            {
                Id = 1,
                Name = "qqq",
                Key = "qeew",
                Description = "ssss",

                Comments = new List<CommentDTO>
                {
                    new CommentDTO()
                },
                GenresIds = new List<int>(),
                PlatformsIds = new List<int>()
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork.GenreRepository.Get()).Returns(It.IsAny<IEnumerable<Genre>>());
            _unitOfWork.Setup(unitOfWork => unitOfWork.PlatformRepository.Get()).Returns(It.IsAny<IEnumerable<Platform>>());
            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Create(It.IsAny<Game>()));
            _unitOfWork.Setup(unit => unit.ImageRepository.Create(It.IsAny<Image>()));
            _mapper.Setup(mapper => mapper.Map<GameDTO, Game>(
                It.IsAny<GameDTO>())).Returns(game);
            _baseService
                .Setup(service => service.LogCreate(It.IsAny<Game>()));

            _gameService.Create(gameDto);
            _unitOfWork.Verify(save => save.Save(), Times.Exactly(2));
        }

        [Test]
        public void GetGameByPlatform_GetPltaform_ReturnNull()
        {
            IEnumerable<Platform> platformList = new List<Platform>();
            string patform = "RPG";
            _unitOfWork.Setup(unitOfWork => unitOfWork.PlatformRepository
                    .Get(It.IsAny<Expression<Func<Platform, bool>>>(),
                        It.IsAny<Expression<Func<Platform, object>>[]>()))
                .Returns(platformList);

            var result = _gameService.GetGamesByPlatform(patform);
            Assert.IsNull(result);
        }

        [Test]
        public void GetGameByPlatform_GetPltaform_ReturnList()
        {
            IEnumerable<Platform> platformList = new List<Platform>
            {
                new Platform
                {
                    Games = new List<Game>
                    {
                        new Game()
                    }
                }
            };
            IEnumerable<GameDTO> gamesDto = new List<GameDTO>
            {
                new GameDTO()
            };
            string patform = "RPG";
            _unitOfWork.Setup(unitOfWork => unitOfWork.PlatformRepository
                    .Get(It.IsAny<Expression<Func<Platform, bool>>>(),
                        It.IsAny<Expression<Func<Platform, object>>[]>()))
                .Returns(platformList);
            _mapper.Setup(map => map.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(gamesDto);

            var result = _gameService.GetGamesByPlatform(patform);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void UpdateForRemove_UpdateGame_SaveWorksNever()
        {
            IEnumerable<Game> game = new List<Game>();
            _unitOfWork.Setup(unit => unit.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(game);
            _baseService
                .Setup(service => service.LogDelete(It.IsAny<Game>()));

            _gameService.UpdateForRemove(1);
            _unitOfWork.Verify(unit => unit.Save(), Times.Never);
        }

        [Test]
        public void UpdateForRemove_UpdateGame_SaveWorksOnce()
        {
            IEnumerable<Game> game = new List<Game>
            {
                new Game()
            };
            IEnumerable<Order> order = new List<Order>();
            _unitOfWork.Setup(unit => unit.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(game);
            _unitOfWork.Setup(unit => unit.OrdersRepository.Get(It.IsAny<Func<Order, bool>>()))
                .Returns(order);
            _baseService
                .Setup(service => service.LogDelete(It.IsAny<Game>()));

            _gameService.UpdateForRemove(1);
            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void Delete_UpdateGame_SaveWorksNever()
        {
            IEnumerable<Game> game = new List<Game>();
            _unitOfWork.Setup(unit => unit.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(game);
            _baseService
                .Setup(service => service.LogDelete(It.IsAny<Game>()));

            _gameService.Delete(1);
            _unitOfWork.Verify(unit => unit.Save(), Times.Never);
        }

        [Test]
        public void Delete_UpdateGame_SaveWorksOnce()
        {
            IEnumerable<Game> game = new List<Game>
            {
                new Game()
            };
            _unitOfWork.Setup(unit => unit.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(game);
            _baseService
                .Setup(service => service.LogDelete(It.IsAny<Game>()));

            _gameService.Delete(1);
            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void Filters_Filters_ReturnNotNull()
        {
            var filter = new FilterDTO
            {
                Games = new List<GameDTO>()
            };
            var gamePipe = new GameSelectionPipeline();
            IEnumerable<Game> games = new List<Game>();
            _unitOfWork.Setup(unit => unit.GameRepository
                    .Filter(gamePipe,1,1))
                .Returns(games);

            var result = _gameService.Filter(filter,1,2);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetGamesByRange_Filters_ReturnNotNull()
        {
            var filter = new FilterDTO
            {
                Games = new List<GameDTO>()
            };
            var gamePipe = new GameSelectionPipeline();
            IEnumerable<Game> games = new List<Game>();
            _unitOfWork.Setup(unit => unit.GameRepository
                    .GetByRange(1, 2))
                .Returns(games);

            var result = _gameService.GetGamesByRange(1, 2);
            Assert.IsNotNull(result);
        }

        [Test]
        public void RegisterFilters_Filters_ReturnNotNull()
        {
            var filter = new FilterDTO
            {
                Games = new List<GameDTO>()
            };
            var gamePipe = new GameSelectionPipeline();
            IEnumerable<Game> games = new List<Game>();
            _unitOfWork.Setup(unit => unit.GameRepository
                    .GetByRange(1, 2))
                .Returns(games);

            var result = _gameService.RegisterFilters(filter);
            Assert.IsNotNull(result);
        }

        [Test]
        public void RegisterFilters_Filters_RegisterAll()
        {
            var filter = new FilterDTO
            {
                Games = new List<GameDTO>(),
                GenresId = new List<int>(1),
                PublisherDateId = 1,
                SearchName = "Bla",
                PlatformsId = new List<int>(1),
                PublishersId = new List<int>(1),
                SortBy = 1
            };
            var gamePipe = new GameSelectionPipeline();
            IEnumerable<Game> games = new List<Game>();
            _unitOfWork.Setup(unit => unit.GameRepository
                    .GetByRange(1, 2))
                .Returns(games);

            var result = _gameService.RegisterFilters(filter);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetDeletedGames_GetGames_GetNotNull()
        {
            IEnumerable<Game> games = new List<Game>();
            _unitOfWork.Setup(unit => unit.GameRepository.GetDeleted()).Returns(games);

            var result = _gameService.GetDeletedGames();

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetDeletedGames_GetGames_GetNotEmpty()
        {
            IEnumerable<Game> games = new List<Game>
            {
                new Game()
            };
            _unitOfWork.Setup(unit => unit.GameRepository.GetDeleted()).Returns(games);

            var result = _gameService.GetDeletedGames();

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetDeletedGameDetails_GetGames_GetNull()
        {
            IEnumerable<Game> games = new List<Game>();
            _unitOfWork.Setup(unit => unit.GameRepository
                    .GetDeleted(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(games);

            var result = _gameService.GetDeletedGameDetails(string.Empty, string.Empty);

            Assert.IsNull(result);
        }

        [Test]
        public void GetDeletedGameDetails_GetGames_GetNotNull()
        {
            IEnumerable<Game> games = new List<Game>
            {
                new Game
                {
                    Publisher = new Publisher(),
                    Genres = new List<Genre>(),
                    Platforms = new List<Platform>()
                }
            };
            var gameDto = new GameDTO
            {
                Publisher = new PublisherDTO(),
                Genres = new List<GenreDTO>(),
                Platforms = new List<PlatformDTO>(),
                GameTranslates = new List<GameTranslateDto>()
            };

            _unitOfWork.Setup(unit => unit.GameRepository
                    .GetDeleted(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(games);
            _unitOfWork.Setup(unit => unit.PublisherRepository.FindById(It.IsAny<int>()))
                .Returns(new Publisher());
            _unitOfWork.Setup(unit => unit.GameRepository.Update(It.IsAny<Game>()));
            _unitOfWork.Setup(unit => unit.ImageRepository
                .Get(It.IsAny<Expression<Func<Image, bool>>>())).Returns(new List<Image>());
            _mapper.Setup(mapper => mapper.Map<Game, GameDTO>(
                It.IsAny<Game>())).Returns(gameDto);

            var result = _gameService.GetDeletedGameDetails(string.Empty, string.Empty);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetDeletedGameDetailsById_GetGames_GetNull()
        {
            IEnumerable<Game> games = new List<Game>();
            _unitOfWork.Setup(unit => unit.GameRepository
                    .GetDeleted(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(games);

            var result = _gameService.GetDeletedGameDetailsById(1, string.Empty);

            Assert.IsNull(result);
        }

        [Test]
        public void GetDeletedGameDetailsById_GetGames_GetNotNull()
        {
            IEnumerable<Game> games = new List<Game>
            {
                new Game
                {
                    Publisher = new Publisher(),
                    Genres = new List<Genre>(),
                    Platforms = new List<Platform>()
                }
            };
            var gameDto = new GameDTO
            {
                Publisher = new PublisherDTO(),
                Genres = new List<GenreDTO>(),
                Platforms = new List<PlatformDTO>(),
                GameTranslates = new List<GameTranslateDto>()
            };

            _unitOfWork.Setup(unit => unit.GameRepository
                    .GetDeleted(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(games);
            _unitOfWork.Setup(unit => unit.PublisherRepository.FindById(It.IsAny<int>()))
                .Returns(new Publisher());
            _unitOfWork.Setup(unit => unit.GameRepository.Update(It.IsAny<Game>()));
            _unitOfWork.Setup(unit => unit.ImageRepository
                .Get(It.IsAny<Expression<Func<Image, bool>>>())).Returns(new List<Image>());
            _mapper.Setup(mapper => mapper.Map<Game, GameDTO>(
                It.IsAny<Game>())).Returns(gameDto);

            var result = _gameService.GetDeletedGameDetailsById(1, string.Empty);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Edit_TestSave_WorksOnce()
        {
            var game = new Game
            {
                Publisher = new Publisher(),
                Genres = new List<Genre>(),
                Platforms = new List<Platform>(),
                GameTranslates = new List<GameTranslate>(),
                PublisherId = 1
            };
            var gameDto = new GameDTO
            {
                Publisher = new PublisherDTO(),
                Genres = new List<GenreDTO>(),
                Platforms = new List<PlatformDTO>(),
                GameTranslates = new List<GameTranslateDto>(),
                GenresIds = new List<int>(),
                PlatformsIds = new List<int>(),
                PublisherId = 1
            };
            _unitOfWork.Setup(unit => unit.GameRepository.Update(It.IsAny<Game>()));
            _unitOfWork.Setup(unit => unit.GameRepository.FindById(It.IsAny<int>()))
                .Returns(game);
            _unitOfWork.Setup(unit => unit.PublisherRepository.FindById(It.IsAny<int>()))
                .Returns(new Publisher());
            _unitOfWork.Setup(unit => unit.ImageRepository
                    .Get(It.IsAny<Expression<Func<Image, bool>>>()))
                .Returns(new List<Image>());
            _baseService.Setup(baseSrvie => baseSrvie.LogUpdate(It.IsAny<Game>(), It.IsAny<Game>()));
            _mapper.Setup(mapper => mapper.Map<Game, GameDTO>(
                It.IsAny<Game>())).Returns(gameDto);

            _gameService.Edit(gameDto);

            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }
    }
}
