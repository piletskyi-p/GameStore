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
    public class PublisherControllerTests
    {
        private readonly Mock<IPublisherService> _publisherService;
        private readonly Mock<ILanguageService> _langService;
        private readonly Mock<IAuthentication> _authService;
        private readonly Mock<ILogger> _logger;
        private PublisherController _publisherController;

        public PublisherControllerTests()
        {
            _publisherService = new Mock<IPublisherService>();
            _logger = new Mock<ILogger>();
            _authService = new Mock<IAuthentication>();
            _langService = new Mock<ILanguageService>();
        }

        [SetUp]
        public void Setup()
        {
            _publisherController = new PublisherController(
                _publisherService.Object,
                _logger.Object,
                _authService.Object,
                _langService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void PublisherDetailsById_GetPublisher_ReturnError()
        {
            var result = _publisherController.PublisherDetailsById(0);
            var ex = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void PublisherDetailsById_GetPublisher_ReturnEr()
        {
            _publisherService.Setup(s => s.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(It.IsAny<PublisherDTO>());
            var ex = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var result = _publisherController.PublisherDetailsById(1);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void PublisherDetailsById_GetPublisher_ReturnCorrectView()
        {
            var pub = new PublisherDTO
            {
                CompanyName = "dsd"
            };
            _publisherService.Setup(s => s.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(pub);

            var result = (ViewResult)_publisherController.PublisherDetailsById(1);
            Assert.AreEqual("PublisherDetails", result.ViewName);
        }

        [Test]
        public void PublisherDetails_GetPublisher_ReturnError()
        {
            var result = _publisherController.PublisherDetails(string.Empty);
            var ex = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void PublisherDetails_GetPublisher_ReturnEr()
        {
            _publisherService.Setup(s => s.GetByName(It.IsAny<string>(), It.IsAny<string>())).Returns(new PublisherDTO());

            var result = (ViewResult)_publisherController.PublisherDetails("ME");
            Assert.AreEqual(typeof(PublisherViewModel), result.Model.GetType());
        }

        [Test]
        public void PublisherDetails_GetPublisher_ReturnCorrectView()
        {
            var pub = new PublisherDTO
            {
                CompanyName = "sfs"
            };
            _publisherService.Setup(s => s.GetByName(It.IsAny<string>(), It.IsAny<string>())).Returns(pub);

            var result = (ViewResult)_publisherController.PublisherDetails("ME");
            Assert.AreEqual("PublisherDetails", result.ViewName);
        }

        [Test]
        public void GetAll_GetPublisher_ReturnError()
        {
            var pubDto = new List<PublisherDTO>();
            _publisherService.Setup(pub => pub.GetAllPublisher(It.IsAny<string>())).Returns(pubDto);

            var result = (ViewResult)_publisherController.GetAll();
            Assert.AreEqual(typeof(List<PublisherViewModel>), result.Model.GetType());
        }

        [Test]
        public void GetAll_GetPublisher_ReturnCorrectView()
        {
            var pubDto = new List<PublisherDTO>
            {
                new PublisherDTO()
            };
            _publisherService.Setup(pub => pub.GetAllPublisher(It.IsAny<string>())).Returns(pubDto);

            var result = (ViewResult)_publisherController.GetAll();
            Assert.AreEqual("AllPublishers", result.ViewName);
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void NewPublisherGet_GetPublisher_ReturnCorrectView()
        {
            var result = _publisherController.NewPublisher();

            Assert.AreEqual("NewPublisher", result.ViewName);
        }

        [Test]
        public void NewPublisherGet_GetPublisher_ReturnNotNullModel()
        {
            var result = _publisherController.NewPublisher();

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void NewPublisherPost_GetPublisher_ReturnCorrectView()
        {
            IEnumerable<PublisherDTO> publishers = new List<PublisherDTO>
            {
                new PublisherDTO()
            };
            _publisherService.Setup(service => service.GetAllPublisher(It.IsAny<string>()))
                .Returns(publishers);

            var result = (ViewResult)_publisherController.NewPublisher(new PublisherViewModel());

            Assert.AreEqual("AllPublishers", result.ViewName);
        }

        [Test]
        public void NewPublisherPost_GetPublisher_ReturnNotNullModel()
        {
            IEnumerable<PublisherDTO> publishers = new List<PublisherDTO>
            {
                new PublisherDTO()
            };
            _publisherService.Setup(service => service.GetAllPublisher(It.IsAny<string>()))
                .Returns(publishers);

            var result = (ViewResult)_publisherController.NewPublisher(new PublisherViewModel());

            Assert.IsNotNull(result.Model);
        }
    }
}
