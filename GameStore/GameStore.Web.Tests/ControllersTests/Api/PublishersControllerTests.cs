using System.Collections.Generic;
using System.Net;
using System.Web.Http.Results;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.DTO.TranslateDto;
using GameStore.Bll.Interfaces;
using GameStore.Web.Controllers.Api;
using GameStore.Web.Models.ViewModels;
using Moq;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests.Api
{
    public class PublishersControllerTests
    {
        private readonly Mock<IPublisherService> _publisherService;
        private readonly Mock<ILanguageService> _langService;
        private PublishersController _publisherController;

        public PublishersControllerTests()
        {
            _publisherService = new Mock<IPublisherService>();
            _langService = new Mock<ILanguageService>();
        }

        [SetUp]
        public void Setup()
        {
            _publisherController = new PublishersController(
                _publisherService.Object,
                _langService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void Get_TestReturn_GetCorrectData()
        {
            var publishers = new List<PublisherDTO>
            {
                new PublisherDTO()
                {
                    Games = new List<GameDTO>()
                }
            };
            _publisherService.Setup(service => service.GetAllPublisher(It.IsAny<string>()))
                .Returns(publishers);

            var result = _publisherController.Get();
            var contentResult = result as NegotiatedContentResult<List<PublisherDTO>>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Count);
        }

        [Test]
        public void GetDetails_TestReturn_GetCorrectData()
        {
            var publisher = new PublisherDTO()
            {
                Games = new List<GameDTO>()
            };

            _publisherService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(publisher);

            var result = _publisherController.GetDetails(1);
            var contentResult = result as NegotiatedContentResult<PublisherDTO>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void GetDetails_TestReturn_GetNotFound()
        {
            _publisherService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(It.IsAny<PublisherDTO>());

            var result = _publisherController.GetDetails(1);

            Assert.AreEqual(result.GetType(), typeof(NotFoundResult));
        }

        [Test]
        public void Post_TestReturn_GetCorrectData()
        {
            _publisherService.Setup(service => service.NewPublisher(It.IsAny<PublisherDTO>()));

            var result = _publisherController.Post(new PublisherViewModel());
            var contentResult = result as NegotiatedContentResult<string>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Put_TestReturn_GetCorrectData()
        {
            var pub = new PublisherDTO
            {
                PublisherTranslate = new List<PublisherTranslateDto>()
            };
            _publisherService.Setup(service => service.EditPublisher(It.IsAny<PublisherDTO>()));
            _langService.Setup(service => service.GetById(It.IsAny<int>()))
                .Returns(new LanguageDto());

            var result = _publisherController.Put(1, new PublisherViewModel());
            var contentResult = result as NegotiatedContentResult<string>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Created, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Delete_TestReturn_GetCorrectData()
        {
            _publisherService.Setup(service => service.RemovePublisherById(It.IsAny<int>()));

            var result = _publisherController.Delete(1);
            var contentResult = result as NegotiatedContentResult<string>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Games_TestReturn_GetCorrectData()
        {
            var publisher = new PublisherDTO()
            {
                Games = new List<GameDTO>()
            };

            _publisherService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(publisher);

            var result = _publisherController.Games(1);
            var contentResult = result as NegotiatedContentResult<List<GameDTO>>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Games_TestReturn_GetNotFound()
        {
            _publisherService.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(It.IsAny<PublisherDTO>());

            var result = _publisherController.Games(1);

            Assert.AreEqual(result.GetType(), typeof(NotFoundResult));
        }
    }
}