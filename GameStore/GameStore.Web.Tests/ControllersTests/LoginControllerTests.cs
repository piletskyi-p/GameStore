using System.Collections.Generic;
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
    public class LoginControllerTests
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IBanService> _banService;
        private readonly Mock<IRoleService> _roleService;
        private readonly Mock<IPublisherService> _publisherService;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<ILanguageService> _languageService;
        private Mock<IAuthentication> _auth;
        private LoginController _loginController;

        public LoginControllerTests()
        {
            _userService = new Mock<IUserService>();
            _roleService = new Mock<IRoleService>();
            _publisherService = new Mock<IPublisherService>();
            _logger = new Mock<ILogger>();
            _languageService = new Mock<ILanguageService>();
            _banService = new Mock<IBanService>();
        }

        [SetUp]
        public void Setup()
        {
            _auth = new Mock<IAuthentication>();
            _loginController = new LoginController(
                _banService.Object,
                _auth.Object,
                _languageService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void IndexGet_GetView_ReturnCorrectView()
        {
            var result = (ViewResult)_loginController.Index();
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public void IndexGet_GetView_ReturnNotNullModel()
        {
            var result = (ViewResult)_loginController.Index();
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void Logout_GetView_RedirectToActionWithCorrectParametrs()
        {
            var result = (RedirectToRouteResult)_loginController.Logout();
            Assert.IsNotEmpty(result.RouteValues.Values);
        }

        [Test]
        public void Logout_GetView_ReturnNotNullModel()
        {
            var result = _loginController.Logout();
            _auth.Verify(auth => auth.LogOut(), Times.Once);
        }

        [Test]
        public void Index_GetView_ExistAction()
        {
            var user = new UserDTO
            {
                Id = 1,
                Name = "Bla",
                Roles = new List<RoleDTO>()
            };
            var login = new LoginView
            {
                Email = "Me",
                Password = "Bla",
            };
            _auth.Setup(auth => auth
                    .Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(user);
            var result = (RedirectToRouteResult)_loginController.Index(login);
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
        }

        [Test]
        public void Index_GetView_ExistController()
        {
            var user = new UserDTO
            {
                Id = 1,
                Name = "Bla",
                Roles = new List<RoleDTO>()
            };
            var login = new LoginView
            {
                Email = "Me",
                Password = "Bla",
            };
            _auth.Setup(auth => auth
                    .Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(user);
            var result = (RedirectToRouteResult)_loginController.Index(login);
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
        }

        [Test]
        public void Index_GetView_ReturnNotNullModel()
        {
            var result = (ViewResult)_loginController.Index();
            Assert.IsNotNull(result.Model);
        }
    }
}
