using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;
using GameStore.Web.Controllers;
using GameStore.Web.Models.ViewModels;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests
{
    [TestFixture]
    public class GameControllerTests
    {
        private readonly Mock<IGenreService> _genreService;
        private readonly Mock<IPlatformService> _platformService;
        private readonly Mock<IPublisherService> _publisherService;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IAuthentication> _auth;
        private readonly Mock<ILanguageService> _lang;
        private readonly Mock<IUserService> _userService;
        private GameController _gameController;
        private Mock<IGameService> _gameService;

        public GameControllerTests()
        {
            _genreService = new Mock<IGenreService>();
            _platformService = new Mock<IPlatformService>();
            _publisherService = new Mock<IPublisherService>();
            _logger = new Mock<ILogger>();
            _mapper = new Mock<IMapper>();
            _auth = new Mock<IAuthentication>();
            _lang = new Mock<ILanguageService>();
            _userService = new Mock<IUserService>();
        }

        [SetUp]
        public void Setup()
        {
            _gameService = new Mock<IGameService>();
            _gameController = new GameController(
                _gameService.Object,
                _platformService.Object,
                _genreService.Object,
                _publisherService.Object,
                _logger.Object,
                _lang.Object,
                _auth.Object, 
                _userService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void GetGameDetailsByKey_GetNotNull_ReturnGame()
        {
            string key = "ME";
            var game = new GameDTO
            {
                Id = 1,
                Key = key,
                Name = "pasha",
                Description = "qqq",
            };
            IEnumerable<GenreDTO> genreDto = new List<GenreDTO>
            {
                new GenreDTO()
            };
            IEnumerable<PlatformDTO> platformDto = new List<PlatformDTO>
            {
                new PlatformDTO()
            };
            PublisherDTO publisher = new PublisherDTO();

            _gameService.Setup(gameService => gameService
                    .GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(game);

            _genreService.Setup(g => g.GetByGameKey(It.IsAny<string>(), It.IsAny<string>())).Returns(genreDto);
            _platformService.Setup(p => p.GetByGameKey(It.IsAny<string>(), It.IsAny<string>())).Returns(platformDto);
            _publisherService.Setup(p => p.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(publisher);

            ViewResult result = (ViewResult)_gameController.GetGameDetailsByKey(key);

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void GetGameDetailsByKey_GetCorrectView_ReturnGame()
        {
            string key = "ME";
            var game = new GameDTO
            {
                Id = 1,
                Key = key,
                Name = "pasha",
                Description = "qqq",
            };
            IEnumerable<GenreDTO> genreDto = new List<GenreDTO>
            {
                new GenreDTO()
            };
            IEnumerable<PlatformDTO> platformDto = new List<PlatformDTO>
            {
                new PlatformDTO()
            };
            PublisherDTO publisher = new PublisherDTO();

            _gameService.Setup(gameService => gameService.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(game);

            _genreService.Setup(g => g.GetByGameKey(It.IsAny<string>(), It.IsAny<string>())).Returns(genreDto);
            _platformService.Setup(p => p.GetByGameKey(It.IsAny<string>(), It.IsAny<string>())).Returns(platformDto);
            _publisherService.Setup(p => p.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(publisher);

            ViewResult result = (ViewResult)_gameController.GetGameDetailsByKey(key);

            Assert.IsNotNull("GameDetails", result.ViewName);
        }

        [Test]
        public void GetGameDetailsByKey_GetNotEmpty_ReturnGame()
        {
            string key = "ME";
            var game = new GameDTO
            {
                Id = 1,
                Key = key,
                Name = "pasha",
                Description = "qqq",
            };
            IEnumerable<GenreDTO> genreDto = new List<GenreDTO>
            {
                new GenreDTO()
            };
            IEnumerable<PlatformDTO> platformDto = new List<PlatformDTO>
            {
                new PlatformDTO()
            };
            PublisherDTO publisher = new PublisherDTO();

            _gameService.Setup(gameService => gameService.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(game);

            _genreService.Setup(g => g.GetByGameKey(It.IsAny<string>(), It.IsAny<string>())).Returns(genreDto);
            _platformService.Setup(p => p.GetByGameKey(It.IsAny<string>(), It.IsAny<string>())).Returns(platformDto);
            _publisherService.Setup(p => p.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(publisher);

            ViewResult result = (ViewResult)_gameController.GetGameDetailsByKey(key);

            Assert.AreNotEqual(It.IsAny<ViewResult>(), result.Model);
        }

        [Test]
        public void GetGameDetailsByKey_ResultEqualType_ReturnNull()
        {
            string key = "ME";
            var game = new GameDTO
            {
                Id = 1,
                Key = key,
                Name = "pasha",
                Description = "qqq",
            };

            _gameService.Setup(gameService => gameService.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(game);
            _logger.Setup(_logger => _logger.Info(It.IsAny<string>()));
            _genreService.Setup(g => g.GetByGameKey(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IEnumerable<GenreDTO>>());
            _platformService.Setup(p => p.GetByGameKey(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IEnumerable<PlatformDTO>>());
            _publisherService.Setup(p => p.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(It.IsAny<PublisherDTO>());
            _mapper.Setup(mapper => mapper.Map<GameDTO, GameDetailsViewModel>(
                It.IsAny<GameDTO>())).Returns(It.IsAny<GameDetailsViewModel>());

            var result = _gameController.GetGameDetailsByKey(string.Empty);
            var ex = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void GetAllGames_CorrectType_ReturnGames()
        {
            string key = "ME";
            var game = new List<GameDTO>
            {
                new GameDTO
                {
                    Id = 1,
                    Key = key,
                    Name = "qqq",
                    Description = "qwe"
                }
            };

            _gameService.Setup(gameService => gameService.GetAllGames())
                .Returns(game);

            var result = (ViewResult)_gameController.GetAllGames();

            Assert.AreEqual(typeof(IndexGameViewModel), result.Model.GetType());
        }

        [Test]
        public void GetAllGames_CorrectView_ReturnGames()
        {
            string key = "ME";
            var game = new List<GameDTO>
            {
                new GameDTO
                {
                    Id = 1,
                    Key = key,
                    Name = "qqq",
                    Description = "qwe"
                }
            };

            _gameService.Setup(gameService => gameService.GetAllGames())
                .Returns(game);

            var result = (ViewResult)_gameController.GetAllGames();

            Assert.AreEqual("AllGames", result.ViewName);
        }

        [Test]
        public void GetAllGames_NotEmptyView_ReturnGames()
        {
            string key = "ME";
            var game = new List<GameDTO>
            {
                new GameDTO
                {
                    Id = 1,
                    Key = key,
                    Name = "qqq",
                    Description = "qwe"
                }
            };

            _gameService.Setup(gameService => gameService.GetAllGames())
                .Returns(game);

            var result = (ViewResult)_gameController.GetAllGames();

            Assert.AreNotEqual(string.Empty, result.ViewName);
        }

        [Test]
        public void GetDeletedGames_NotEmptyView_ReturnCorrectView()
        {
            string key = "ME";
            var game = new List<GameDTO>
            {
                new GameDTO
                {
                    Id = 1,
                    Key = key,
                    Name = "qqq",
                    Description = "qwe"
                }
            };
            _gameService.Setup(gameService => gameService.GetDeletedGames())
                .Returns(game);

            var result = (ViewResult)_gameController.GetDeletedGames();

            Assert.AreEqual("AllGames", result.ViewName);
        }

        [Test]
        public void GetDeletedGames_NotEmptyView_ReturnNotNullModel()
        {
            string key = "ME";
            var game = new List<GameDTO>
            {
                new GameDTO
                {
                    Id = 1,
                    Key = key,
                    Name = "qqq",
                    Description = "qwe"
                }
            };
            _gameService.Setup(gameService => gameService.GetDeletedGames())
                .Returns(game);

            var result = (ViewResult)_gameController.GetDeletedGames();

            Assert.NotNull(result.Model);
        }

        [Test]
        public void DownloadGame_GetEx_ReturnNull()
        {
            var result = _gameController.DownloadGame(string.Empty);

            var ex = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void Remove_DeleteGame_WorkOnce()
        {
            IEnumerable<GameDTO> g = new List<GameDTO>();
            int id = 1;
            _gameService.Setup(game => game.GetAllGames()).Returns(g);
            _mapper.Setup(mapper => mapper.Map<IEnumerable<GameDTO>, IEnumerable<GameViewModel>>(
                It.IsAny<IEnumerable<GameDTO>>())).Returns(It.IsAny<IEnumerable<GameViewModel>>());

            _gameController.Remove(id);
            _gameService.Verify(gameService => gameService.UpdateForRemove(id), Times.Once());
        }

        [Test]
        public void DeleteGame_TryToDeleteGameWithoutID_ReturnNull()
        {
            var result = _gameController.Remove(0);
            var ex = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void GetGameCount_CheckCount_ReturnNotNull()
        {
            _gameService.Setup(game => game.GetAllGames()).Returns(new List<GameDTO>());

            var res = _gameController.GetGameCount();
            Assert.NotNull(res);
        }

        [Test]
        public void GetGameCount_CheckCount_ReturnZero()
        {
            _gameService.Setup(game => game.GetAllGames()).Returns(new List<GameDTO>());

            var res = _gameController.GetGameCount();
            Assert.AreEqual(0, res);
        }

        [Test]
        public void GetGameCount_CheckCount_ReturnCorrectNumber()
        {
            var games = new List<GameDTO>
            {
                new GameDTO(),
                new GameDTO()
            };
            _gameService.Setup(game => game.GetAllGames()).Returns(games);

            var res = _gameController.GetGameCount();
            Assert.AreEqual(games.Count, res);
        }

        [Test]
        public void EditGameGet_CheckCount_ReturnNotNullModel()
        {
            var games = new GameDTO
            {
                Id = 1,
                Platforms = new List<PlatformDTO>
                {
                    new PlatformDTO()
                },
                Genres = new List<GenreDTO>(),
                PlatformsIds = new List<int>(),
                GenresIds = new List<int>()
            };
            _gameService.Setup(game => game.GetItemById(1, It.IsAny<string>())).Returns(games);
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>())).Returns(new List<PlatformDTO> { new PlatformDTO() });

            var res = (ViewResult)_gameController.EditGame(1);
            Assert.NotNull(res.Model);
        }

        [Test]
        public void EditGameGet_CheckCount_ReturnNotCorrectView()
        {
            var games = new GameDTO
            {
                Id = 1,
                Platforms = new List<PlatformDTO>
                {
                    new PlatformDTO()
                },
                Genres = new List<GenreDTO>(),
                PlatformsIds = new List<int>(),
                GenresIds = new List<int>()
            };
            _gameService.Setup(game => game.GetItemById(1, It.IsAny<string>())).Returns(games);
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>())).Returns(new List<PlatformDTO> { new PlatformDTO() });

            var res = (ViewResult)_gameController.EditGame(1);
            Assert.AreEqual("EditGame", res.ViewName);
        }

        [Test]
        public void FilterGet_CheckCount_ReturnNotCorrectView()
        {
            var games = new GameDTO
            {
                Id = 1,
                Platforms = new List<PlatformDTO>
                {
                    new PlatformDTO()
                },
                Genres = new List<GenreDTO>(),
                PlatformsIds = new List<int>(),
                GenresIds = new List<int>()
            };
            _gameService.Setup(game => game.GetItemById(1, It.IsAny<string>())).Returns(games);
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>())).Returns(new List<PlatformDTO> { new PlatformDTO() });
            FilterViewModel filter = new FilterViewModel();

            var res = (ViewResult)_gameController.Filter(filter);
            Assert.AreEqual("GameFilters", res.ViewName);
        }

        [Test]
        public void FilterGet_CheckCount_ReturnNotNull()
        {
            var games = new GameDTO
            {
                Id = 1,
                Platforms = new List<PlatformDTO>
                {
                    new PlatformDTO()
                },
                Genres = new List<GenreDTO>(),
                PlatformsIds = new List<int>(),
                GenresIds = new List<int>()
            };
            _gameService.Setup(game => game.GetItemById(1, It.IsAny<string>())).Returns(games);
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>())).Returns(new List<PlatformDTO> { new PlatformDTO() });
            FilterViewModel filter = new FilterViewModel();

            var res = (ViewResult)_gameController.Filter(filter);
            Assert.NotNull(res.Model);
        }

        [Test]
        public void Filters_CheckCount_ReturnNotNull()
        {
            var games = new GameDTO
            {
                Id = 1,
                Platforms = new List<PlatformDTO>
                {
                    new PlatformDTO()
                },
                Genres = new List<GenreDTO>(),
                PlatformsIds = new List<int>(),
                GenresIds = new List<int>()
            };
            _gameService.Setup(game => game.GetItemById(1, It.IsAny<string>())).Returns(games);
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>())).Returns(new List<PlatformDTO> { new PlatformDTO() });
            _gameService.Setup(service => service.FilterCount(It.IsAny<FilterDTO>())).Returns(0);
            _gameService.Setup(service => service
                .Filter(It.IsAny<FilterDTO>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<GameDTO>());

            var res = (ViewResult)_gameController.Filters(new FilterViewModel());
            Assert.NotNull(res.Model);
        }

        [Test]
        public void Filters_CheckCount_ReturnCorrectView()
        {
            var games = new GameDTO
            {
                Id = 1,
                Platforms = new List<PlatformDTO>
                {
                    new PlatformDTO()
                },
                Genres = new List<GenreDTO>(),
                PlatformsIds = new List<int>(),
                GenresIds = new List<int>()
            };
            _gameService.Setup(game => game.GetItemById(1, It.IsAny<string>())).Returns(games);
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>())).Returns(new List<PlatformDTO> { new PlatformDTO() });
            _gameService.Setup(service => service.FilterCount(It.IsAny<FilterDTO>())).Returns(0);
            _gameService.Setup(service => service
                    .Filter(It.IsAny<FilterDTO>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<GameDTO>());

            var res = (ViewResult)_gameController.Filters(new FilterViewModel());
            Assert.AreEqual("GameFilters", res.ViewName);
        }

        [Test]
        public void NewGame_CheckCount_ReturnCorrectViewIfModelStateIsFalse()
        {
            var games = new GameDTO
            {
                Id = 1,
                Platforms = new List<PlatformDTO>
                {
                    new PlatformDTO()
                },
                Genres = new List<GenreDTO>(),
                PlatformsIds = new List<int>(),
                GenresIds = new List<int>()
            };
            _gameService.Setup(game => game.GetItemById(1, It.IsAny<string>())).Returns(games);
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>())).Returns(new List<PlatformDTO> { new PlatformDTO() });
            _gameService.Setup(service => service.FilterCount(It.IsAny<FilterDTO>())).Returns(0);
            _gameService.Setup(service => service
                    .Filter(It.IsAny<FilterDTO>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<GameDTO>());

            var res = (ViewResult)_gameController.NewGame(new GameViewModel());
            Assert.AreEqual("GameFilters", res.ViewName);
        }

        [Test]
        public void NewGame_CheckCount_ReturnCorrectView()
        {
            var games = new GameDTO
            {
                Id = 1,
                Platforms = new List<PlatformDTO>
                {
                    new PlatformDTO()
                },
                Genres = new List<GenreDTO>(),
                PlatformsIds = new List<int>(),
                GenresIds = new List<int>()
            };
            var param = new GameViewModel
            {
                Key = "dfefe",
                Price = 12,
                UnitInStock = 12,
                GenresIds = new List<int> { 1,2},
                PlatformsIds = new List<int> { 1,2},
                PublicationDate = DateTime.UtcNow
            };

            _gameService.Setup(game => game.GetItemById(1, It.IsAny<string>())).Returns(games);
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>())).Returns(new List<PlatformDTO> { new PlatformDTO() });
            _gameService.Setup(service => service.FilterCount(It.IsAny<FilterDTO>())).Returns(0);
            _gameService.Setup(service => service
                    .Filter(It.IsAny<FilterDTO>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<GameDTO>());

            var res = (ViewResult)_gameController.NewGame(param);
            Assert.AreEqual("GameFilters", res.ViewName);
        }

        [Test]
        public void EditGame_CheckCount_ReturnCorrectViewIfModelStateIsFalse()
        {
            var games = new GameDTO
            {
                Id = 1,
                Platforms = new List<PlatformDTO>
                {
                    new PlatformDTO()
                },
                Genres = new List<GenreDTO>(),
                PlatformsIds = new List<int>(),
                GenresIds = new List<int>()
            };
            _gameService.Setup(game => game.GetItemById(1, It.IsAny<string>())).Returns(games);
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>())).Returns(new List<PlatformDTO> { new PlatformDTO() });
            _gameService.Setup(service => service.FilterCount(It.IsAny<FilterDTO>())).Returns(0);
            _gameService.Setup(service => service
                    .Filter(It.IsAny<FilterDTO>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<GameDTO>());

            var res = (ViewResult)_gameController.EditGame(new GameViewModel());
            Assert.AreEqual("GameFilters", res.ViewName);
        }

        [Test]
        public void EditGame_CheckCount_ReturnCorrectView()
        {
            var games = new GameDTO
            {
                Id = 1,
                Platforms = new List<PlatformDTO>
                {
                    new PlatformDTO()
                },
                Genres = new List<GenreDTO>(),
                PlatformsIds = new List<int>(),
                GenresIds = new List<int>()
            };
            var param = new GameViewModel
            {
                Key = "dfefe",
                Price = 12,
                UnitInStock = 12,
                GenresIds = new List<int> { 1, 2 },
                PlatformsIds = new List<int> { 1, 2 },
                PublicationDate = DateTime.UtcNow
            };

            _gameService.Setup(game => game.GetItemById(1, It.IsAny<string>())).Returns(games);
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>())).Returns(new List<PlatformDTO> { new PlatformDTO() });
            _gameService.Setup(service => service.FilterCount(It.IsAny<FilterDTO>())).Returns(0);
            _gameService.Setup(service => service
                    .Filter(It.IsAny<FilterDTO>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<GameDTO>());

            var res = (ViewResult)_gameController.EditGame(param);
            Assert.AreEqual("GameFilters", res.ViewName);
        }
    }
}