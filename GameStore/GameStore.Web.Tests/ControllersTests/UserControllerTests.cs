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
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IRoleService> _roleService;
        private readonly Mock<IPublisherService> _publisherService;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<ILanguageService> _languageService;
        private Mock<IAuthentication> _auth;
        private UserController _userController;

        public UserControllerTests()
        {
            _userService = new Mock<IUserService>();
            _roleService = new Mock<IRoleService>();
            _publisherService = new Mock<IPublisherService>();
            _logger = new Mock<ILogger>();
            _auth = new Mock<IAuthentication>();
            _languageService = new Mock<ILanguageService>();
        }

        [SetUp]
        public void Setup()
        {
            _userController = new UserController(
                _userService.Object,
                _logger.Object,
                _roleService.Object,
                _publisherService.Object,
                _auth.Object,
                _languageService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void UserPage_GetView_ReturnCorrectView()
        {
            _userService.Setup(service => service.GetUser(It.IsAny<string>()))
                .Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name)
                .Returns("ME");

            var result = (ViewResult)_userController.UserPage();

            Assert.AreEqual("UserPage", result.ViewName);
        }

        [Test]
        public void UserPage_GetView_ReturnNotNullModel()
        {
            _userService.Setup(service => service.GetUser(It.IsAny<string>()))
                .Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name)
                .Returns("ME");

            var result = (ViewResult)_userController.UserPage();

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void RegisterGet_GetView_ReturnNotNullModel()
        {
            var result = (ViewResult)_userController.Register();

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void RegisterGet_GetView_ReturnCorrectView()
        {
            var result = (ViewResult)_userController.Register();

            Assert.AreEqual("Register", result.ViewName);
        }

        [Test]
        public void Register_GetView_ReturnNotNullModelIfUserExist()
        {
            _userService.Setup(service => service.GetUser(It.IsAny<string>()))
                .Returns(new UserDTO());

            var result = (ViewResult)_userController.Register(new UserViewModel());

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void Register_GetView_ReturnCorrectViewIfUserExist()
        {
            _userService.Setup(service => service.GetUser(It.IsAny<string>()))
                .Returns(new UserDTO());

            var result = (ViewResult)_userController.Register(new UserViewModel());

            Assert.AreEqual("Register", result.ViewName);
        }

        [Test]
        public void Register_GetView_ReturnNotNullModelIfUserNotExist()
        {
            _userService.Setup(service => service.GetUser(It.IsAny<string>()))
                .Returns(It.IsAny<UserDTO>());

            var result = (RedirectToRouteResult)_userController.Register(new UserViewModel{Password = "me"});

            Assert.IsNotNull(result);
        }

        [Test]
        public void Register_GetView_RedirectToActionWithNotNullParametrs()
        {
            _userService.Setup(service => service.GetUser(It.IsAny<string>()))
                .Returns(It.IsAny<UserDTO>());

            var result = (RedirectToRouteResult)_userController.Register(new UserViewModel { Password = "me" });

            Assert.IsNotEmpty(result.RouteValues.Values);
        }

        [Test]
        public void GetAllUsers_GetView_GetCorrectView()
        {
            IEnumerable<UserDTO> list = new List<UserDTO>
            {
                new UserDTO()
            };
            _userService.Setup(service => service.GetAllUsers())
                .Returns(list);

            var result = (ViewResult)_userController.GetAllUsers();

            Assert.AreEqual("AllUsers", result.ViewName);
        }

        [Test]
        public void GetAllUsers_GetView_ReturnNotNullModel()
        {
            IEnumerable<UserDTO> list = new List<UserDTO>
            {
                new UserDTO()
            };
            _userService.Setup(service => service.GetAllUsers())
                .Returns(list);

            var result = (ViewResult)_userController.GetAllUsers();

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void GetAllUsers_GetView_ReturnERROR()
        {
            IEnumerable<UserDTO> list = new List<UserDTO>();
            _userService.Setup(service => service.GetAllUsers())
                .Returns(list);
            var error = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var result = _userController.GetAllUsers();

            Assert.AreEqual(error.GetType(), result.GetType());
        }

        [Test]
        public void EditUserGet_GetView_ReturnCorrectView()
        {
            _userService.Setup(service => service.GetUserById(1))
                .Returns(new UserDTO());
            var result = (ViewResult)_userController.EditUser(1);

            Assert.AreEqual("EditUser", result.ViewName);
        }

        [Test]
        public void EditUserGet_GetView_ReturnNotNullModel()
        {
            _userService.Setup(service => service.GetUserById(1))
                .Returns(new UserDTO());
            var result = (ViewResult)_userController.EditUser(1);

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void EditUserGet_GetView_ReturnError()
        {
            _userService.Setup(service => service.GetUserById(1))
                .Returns(It.IsAny<UserDTO>());
            var error = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var result = _userController.EditUser(1);

            Assert.AreEqual(error.GetType(), result.GetType());
        }

        [Test]
        public void EditUser_GetView_RedirectToActionWithNotNullParametrs()
        {
            _userService.Setup(service => service.GetUserById(1))
                .Returns(It.IsAny<UserDTO>());
            
            var result = (RedirectToRouteResult)_userController.EditUser(It.IsAny<EditUserViewModel>());
            Assert.IsNotEmpty(result.RouteValues);
        }
    }
}
