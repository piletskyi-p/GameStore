using System.Net;
using System.Web.Http.Results;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Controllers.Api;
using GameStore.Web.Models;
using Moq;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests.Api
{
    public class LoginsControllerTests
    {
        private readonly Mock<IUserTokenService> _userTokenService;
        private readonly Mock<IUserService> _userService;
        private LoginsController _loginsController;

        public LoginsControllerTests()
        {
            _userTokenService = new Mock<IUserTokenService>();
            _userService = new Mock<IUserService>();
        }

        [SetUp]
        public void Setup()
        {
            _loginsController = new LoginsController(
                _userService.Object,
                _userTokenService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void Login_GetError()
        {
            _userService.Setup(service => service.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<UserDTO>());

            var result = _loginsController.Login(new LoginUser());
            var contentResult = result as NegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Unauthorized, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Login_GetCorrectData()
        {
            var login = new LoginUser
            {
                Email = "email",
                Password = "12345"
            };
            var user = new UserDTO
            {
                Email = "email"
            };
            _userService.Setup(service => service.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(user);
            _userTokenService.Setup(service => service.Create(It.IsAny<UserTokenDto>()));
            _userTokenService.Setup(service => service.GetByUserId(It.IsAny<int>()))
                .Returns(new UserTokenDto());

            var result = _loginsController.Login(login);
            var contentResult = result as NegotiatedContentResult<UserTokenDto>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }
    }
}