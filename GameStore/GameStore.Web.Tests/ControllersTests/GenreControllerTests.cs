using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;
using GameStore.Web.Controllers;
using GameStore.Web.Models.LanguageModels;
using GameStore.Web.Models.ViewModels;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests
{
    public class GenreControllerTests
    {
        private readonly Mock<IGenreService> _genreService;
        private readonly Mock<IPlatformService> _platformService;
        private readonly Mock<IPublisherService> _publisherService;
        private readonly Mock<ILanguageService> _langService;
        private readonly Mock<IAuthentication> _authService;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IMapper> _mapper;
        private GenreController _genreController;
        private Mock<IGameService> _gameService;

        public GenreControllerTests()
        {
            _genreService = new Mock<IGenreService>();
            _platformService = new Mock<IPlatformService>();
            _publisherService = new Mock<IPublisherService>();
            _logger = new Mock<ILogger>();
            _mapper = new Mock<IMapper>();
            _authService = new Mock<IAuthentication>();
            _langService = new Mock<ILanguageService>();
        }

        [SetUp]
        public void Setup()
        {
            _gameService = new Mock<IGameService>();
            _genreController = new GenreController(
                _gameService.Object,
                _platformService.Object,
                _genreService.Object,
                _publisherService.Object,
                _logger.Object,
            _authService.Object,
            _langService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void GetAllGenres_GetGenres_ReturnNotNull()
        {
            var genreDto = new List<GenreDTO>
            {
                new GenreDTO()
            };
            _genreService.Setup(service => service.GetAll(It.IsAny<string>()))
                .Returns(genreDto);

            var result = (ViewResult)_genreController.GetAllGenres();
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void GetAllGenres_GetGenres_ReturnCorectView()
        {
            var genreDto = new List<GenreDTO>
            {
                new GenreDTO()
            };
            _genreService.Setup(service => service.GetAll(It.IsAny<string>()))
                .Returns(genreDto);

            var result = (ViewResult)_genreController.GetAllGenres();
            Assert.AreEqual("AllGenres", result.ViewName);
        }

        [Test]
        public void EditGet_Test_ReturnError()
        {
            _genreService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(It.IsAny<GenreDTO>());

            var result = _genreController.Edit(1);
            var ex = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void EditGet_Test_ReturnNotNull()
        {
            var genreDto = new GenreDTO();
            _genreService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(genreDto);

            var result = (ViewResult)_genreController.Edit(1);
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void EditPost_Test_ReturnCorrectView()
        {
            var genreDto = new GenreViewModel
            {
                GenreTranslates = new List<GenreTranslateModel>()
            };
            _langService.Setup(service => service.GetById(It.IsAny<int>()))
                .Returns(It.IsAny<LanguageDto>());

            var result = (ViewResult)_genreController.Edit(new GenreViewModel());
            Assert.AreEqual("AllGenres", result.ViewName);
        }

        [Test]
        public void EditGet_Test_ReturnCorrectView()
        {
            var genreDto = new GenreDTO();
            _genreService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(genreDto);

            var result = (ViewResult)_genreController.Edit(1);
            Assert.AreEqual("EditGenre", result.ViewName);
        }

        [Test]
        public void AddGenre_Test_ReturnCorrectView()
        {
            var result = (ViewResult)_genreController.NewGenre(1);
            Assert.AreEqual("NewGenre", result.ViewName);
        }

        [Test]
        public void AddGenre_Test_ReturnNotNull()
        {
            var result = (ViewResult)_genreController.NewGenre(1);
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void Remove_Test_ReturnError()
        {
            var result = _genreController.Remove(0);
            var ex = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }
    }
}
