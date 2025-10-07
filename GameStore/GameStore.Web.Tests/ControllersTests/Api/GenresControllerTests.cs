using System.Collections.Generic;
using System.Net;
using System.Web.Http.Results;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Controllers.Api;
using GameStore.Web.Models.ViewModels;
using Moq;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests.Api
{
    public class GenresControllerTests
    {
        private readonly Mock<IGenreService> _genreService;
        private readonly Mock<ILanguageService> _langService;
        private readonly Mock<IGameService> _gameService;
        private GenresController _genresController;

        public GenresControllerTests()
        {
            _genreService = new Mock<IGenreService>();
            _langService = new Mock<ILanguageService>();
            _gameService = new Mock<IGameService>();
        }

        [SetUp]
        public void Setup()
        {
            _genresController = new GenresController(
                _genreService.Object,
                _langService.Object,
                _gameService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void Get_TestReturn_GetCorrectData()
        {
            var genres = new List<GenreDTO>
            {
                new GenreDTO
                {
                    Games = new List<GameDTO>()
                }
            };
            _genreService.Setup(service => service.GetAll(It.IsAny<string>()))
                .Returns(genres);

            var result = _genresController.Get();
            var contentResult = result as NegotiatedContentResult<List<GenreDTO>>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Count);
        }

        [Test]
        public void GetDetails_TestReturn_GetCorrectData()
        {
            var genre = new GenreDTO
            {
                Games = new List<GameDTO>()
            };

            _genreService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(genre);

            var result = _genresController.GetDetails(1);
            var contentResult = result as NegotiatedContentResult<GenreDTO>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void GetDetails_TestReturn_GetNotFound()
        {
            _genreService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(It.IsAny<GenreDTO>());

            var result = _genresController.GetDetails(1);

            Assert.AreEqual(result.GetType(), typeof(NotFoundResult));
        }

        [Test]
        public void Put_TestReturn_GetCorrectData()
        {
            _genreService.Setup(service => service.Create(It.IsAny<GenreDTO>()));

            var result = _genresController.Put(1, new GenreViewModel());
            var contentResult = result as NegotiatedContentResult<string>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Created, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Post_TestReturn_GetCorrectData()
        {
            _genreService.Setup(service => service.Update(It.IsAny<GenreDTO>()));

            var result = _genresController.Post(new GenreViewModel());
            var contentResult = result as NegotiatedContentResult<string>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Delete_TestReturn_GetCorrectData()
        {
            _genreService.Setup(service => service.Delete(It.IsAny<int>()));

            var result = _genresController.Delete(1);
            var contentResult = result as NegotiatedContentResult<string>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Games_TestReturn_GetCorrectData()
        {
            var games = new List<GameDTO>();

            _gameService.Setup(service => service
                    .GetGamesByGenreId(It.IsAny<int>()))
                .Returns(games);

            var result = _genresController.Games(1);
            var contentResult = result as NegotiatedContentResult<List<GameDTO>>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Games_TestReturn_GetNotFound()
        {
            _gameService.Setup(service => service
                    .GetGamesByGenreId( It.IsAny<int>()))
                .Returns(It.IsAny<List<GameDTO>>());

            var result = _genresController.Games(0);

            Assert.AreEqual(result.GetType(), typeof(BadRequestResult));
        }
    }
}