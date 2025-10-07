using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using GameStore.Web;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.Bll.Tests
{
    [TestFixture]
    public class GameServiceTests
    {
        private GameService _gameService;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ILogger> _logger;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger>();
            _gameService = new GameService(_logger.Object, _unitOfWork.Object);
            _mapper = new Mock<IMapper>();

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
                new GameDTO
                {
                    Comments = new List<CommentDTO>()
                }
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork
                .GameRepository.Get()).Returns(game);
            _mapper.Setup(mapper => mapper
                .Map<IEnumerable<Game>, IEnumerable<GameDTO>>(
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
            _mapper.Setup(mapper => mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(
                It.IsAny<IEnumerable<Game>>())).Returns(resultList);

            var result = _gameService.GetAllGames();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetGameByKey_GetGameByKey_ReturnsGame()
        {
            var localresult = new GameDTO();
            string key = "ME";
            IEnumerable<Game> game = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Key = key,
                    Name = "Mass Effect",
                    Description = "game"
                }
            };
            
            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);
            _unitOfWork.Setup(publisher => publisher.PublisherRepository
                .Get(It.IsAny<Func<Publisher, bool>>()))
                .Returns(new List<Publisher>());
            _mapper.Setup(mapper => mapper.Map<Game, GameDTO>(
                It.IsAny<Game>())).Returns(localresult);

            var result = _gameService.GetGameByKey(key);

            Assert.AreEqual(key, result.Key);
        }

        [Test]
        public void GetGameByKey_GetNotNull_ReturnsGame()
        {
            var resultL = new GameDTO();
            string key = "ME";
            IEnumerable<Game> game = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Key = key,
                    Name = "Mass Effect",
                    Description = "game"
                }
            };

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);
            _unitOfWork.Setup(publisher => publisher.PublisherRepository
                    .Get(It.IsAny<Func<Publisher, bool>>()))
                .Returns(new List<Publisher>());
            _mapper.Setup(mapper => mapper.Map<Game, GameDTO>(
                It.IsAny<Game>())).Returns(resultL);

            var result = _gameService.GetGameByKey(key);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetGameByKey_GetNull_ReturnsGame()
        {
           var resultL = new GameDTO();
            string key = string.Empty;
            var game = new List<Game>();
            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);
            _mapper.Setup(mapper => mapper.Map<Game, GameDTO>(
                It.IsAny<Game>())).Returns(resultL);

            var result = _gameService.GetGameByKey(key);

            Assert.IsNull(result);
        }

        [Test]
        public void GetGameByGenre_GetNullIfGenreIsEmpty_ReturnsGames()
        {
            IEnumerable<GameDTO> resultList = new List<GameDTO>();
            string genre = string.Empty;
            var game = new List<Genre>();

            _unitOfWork.Setup(unitOfWork => unitOfWork.GenreRepository.Get(It.IsAny<Func<Genre, bool>>()))
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
                    Name = "test",
                    Games = new List<Game> { new Game() }
                }
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork.GenreRepository.Get(It.IsAny<Func<Genre, bool>>()))
                .Returns(game);
            _mapper.Setup(mapper => mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(
                It.IsAny<IEnumerable<Game>>())).Returns(resultList);

            resultList = _gameService.GetGamesByGenre(genre);
            Assert.IsNotEmpty(resultList);
        }

        [Test]
        public void GetItemById_GetGameWithNeededID_ReturnsGame()
        {
            IEnumerable<GameDTO> resultList = new List<GameDTO>();
            var game = new List<Game>
            {
                    new Game
                    {
                        Id = 1,
                        Key = "ddd",
                        Name = "Mass Effect",
                        Description = "game"
                    }
            };

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);
            _mapper.Setup(mapper => mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(
                It.IsAny<IEnumerable<Game>>())).Returns(resultList);

            var result = _gameService.GetItemById(1);

            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void GetItemById_GetNullIfIdNotExist_ReturnsGame()
        {
            IEnumerable<GameDTO> resultList = new List<GameDTO>();
            var game = new List<Game>();

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);
            _mapper.Setup(mapper => mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(
                It.IsAny<IEnumerable<Game>>())).Returns(resultList);

            var result = _gameService.GetItemById(2);

            Assert.IsNull(result);
        }

        [Test]
        public void Create_GameRepositoryCreate_WorkNever()
        {
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
            _unitOfWork.Setup(unitOfWork => unitOfWork.PublisherRepository.Get()).Returns(It.IsAny<IEnumerable<Publisher>>());
             _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Create(It.IsAny<Game>()));

            _gameService.Create(gameDto);
            _unitOfWork.Verify(save => save.Save(), Times.Once);
        }

        [Test]
        public void UpdateForRemove_TestSave_Never()
        {
            IEnumerable<Game> game = new List<Game>();
            _unitOfWork.Setup(unitOfWork => unitOfWork
                .GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);

             _gameService.UpdateForRemove(1);
            _unitOfWork.Verify(method => method.Save(), Times.Never);
        }

        [Test]
        public void UpdateForRemove_TestSave_Once()
        {
            IEnumerable<Game> game = new List<Game>
            {
                new Game()
            };
            IEnumerable<Order> orders = new List<Order>();
            _unitOfWork.Setup(unitOfWork => unitOfWork
                    .GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);
            _unitOfWork.Setup(order => order.OrdersRepository.Get())
                .Returns(orders);

            _gameService.UpdateForRemove(1);
            _unitOfWork.Verify(method => method.Save(), Times.Once);
        }

        [Test]
        public void UpdateForRemove_OneMoreWay_Once()
        {
            IEnumerable<Game> game = new List<Game>
            {
                new Game()
            };
            IEnumerable<Order> orders = new List<Order>
            {
                new Order
                {
                    OrderDetails = new List<OrderDetails>()
                }
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork
                    .GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);
            _unitOfWork.Setup(order => order.OrdersRepository.Get())
                .Returns(orders);

            _gameService.UpdateForRemove(1);
            _unitOfWork.Verify(method => method.Save(), Times.Once);
        }
    }
}
