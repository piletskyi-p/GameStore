using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Bll.Services;
using GameStore.Web.Auth;
using GameStore.Web.Controllers;
using GameStore.Web.Models.LanguageModels;
using GameStore.Web.Models.ViewModels;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests
{
    public class PlatformControllerTests
    {
        private readonly Mock<IGenreService> _genreService;
        private readonly Mock<IPlatformService> _platformService;
        private readonly Mock<IPublisherService> _publisherService;
        private readonly Mock<ILanguageService> _langService;
        private readonly Mock<IAuthentication> _authService;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IMapper> _mapper;
        private PlatformController _platformController;
        private Mock<IGameService> _gameService;

        public PlatformControllerTests()
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
            _platformController = new PlatformController(
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
        public void GetAllPlatform_GetPlatforms_ReturnNotNull()
        {
            var platformDto = new List<PlatformDTO>
            {
                new PlatformDTO()
            };
            _platformService.Setup(service => service.GetAll(It.IsAny<string>()))
                .Returns(platformDto);

            var result = (ViewResult)_platformController.GetAllPlatforms();
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void GetAllGenres_GetGenres_ReturnCorectView()
        {
            var platformDto = new List<PlatformDTO>
            {
                new PlatformDTO()
            };
            _platformService.Setup(service => service.GetAll(It.IsAny<string>()))
                .Returns(platformDto);

            var result = (ViewResult)_platformController.GetAllPlatforms();
            Assert.AreEqual("AllPlatforms", result.ViewName);
        }

        [Test]
        public void EditGet_Test_ReturnError()
        {
            _platformService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(It.IsAny<PlatformDTO>());

            var result = _platformController.Edit(1);
            var ex = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void EditGet_Test_ReturnNotNull()
        {
            var platformDto = new PlatformDTO();
            _platformService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                 .Returns(platformDto);

            var result = (ViewResult)_platformController.Edit(1);
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void EditGet_Test_ReturnCorrectView()
        {
            var platformDto = new PlatformDTO();
            _platformService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(platformDto);

            var result = (ViewResult)_platformController.Edit(1);
            Assert.AreEqual("EditPlatform", result.ViewName);
        }

        [Test]
        public void AddPlatform_Test_ReturnCorrectView()
        {
            var result = (ViewResult)_platformController.NewPlatform();
            Assert.AreEqual("NewPlatform", result.ViewName);
        }

        [Test]
        public void AddPlatform_Test_ReturnNotNull()
        {
            var result = (ViewResult)_platformController.NewPlatform();
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void Remove_Test_ReturnError()
        {
            var result = _platformController.Remove(0);
            var ex = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void NewPlatformGet_TestModel_GetNotNull()
        {
            var result = (ViewResult)_platformController.NewPlatform();

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void NewPlatformGet_TestModel_GetCorrectView()
        {
            var result = (ViewResult)_platformController.NewPlatform();

            Assert.AreEqual("NewPlatform", result.ViewName);
        }

        [Test]
        public void NewPlatformPost_TestModel_AddNewCorrectView()
        {
            var platform = new PlatformViewModel
            {
                PlatformTranslates = new List<PlatformTranslateModel>()
            };
            var platforms = new List<PlatformDTO>
            {
                new PlatformDTO()
            };
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>()))
                .Returns(platforms);

            var result = (ViewResult)_platformController.NewPlatform(platform);

            Assert.AreEqual("AllPlatforms", result.ViewName);
        }

        [Test]
        public void NewPlatformPost_TestModel_AddNewNotNullModel()
        {
            var platform = new PlatformViewModel
            {
                PlatformTranslates = new List<PlatformTranslateModel>()
            };
            var platforms = new List<PlatformDTO>
            {
                new PlatformDTO()
            };
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>()))
                .Returns(platforms);

            var result = (ViewResult)_platformController.NewPlatform(platform);

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void EditPost_Test_ReturnNotNull()
        {
            var platform = new PlatformViewModel();
            _platformService.Setup(service => service.Update(It.IsAny<PlatformDTO>()));
            var platforms = new List<PlatformDTO>
            {
                new PlatformDTO()
            };
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>()))
                .Returns(platforms);

            var result = (ViewResult)_platformController.Edit(platform);

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void EditGetPost_Test_ReturnCorrectView()
        {
            var platform = new PlatformViewModel();
            _platformService.Setup(service => service.Update(It.IsAny<PlatformDTO>()));
            var platforms = new List<PlatformDTO>
            {
                new PlatformDTO()
            };
            _platformService.Setup(platf => platf.GetAll(It.IsAny<string>()))
                .Returns(platforms);

            var result = (ViewResult)_platformController.Edit(platform);

            Assert.AreEqual("AllPlatforms", result.ViewName);
        }
    }
}