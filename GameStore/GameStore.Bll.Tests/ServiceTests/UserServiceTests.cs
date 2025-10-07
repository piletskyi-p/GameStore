using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using GameStore.Web;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<NLog.ILogger> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private UserService _userService;

        public UserServiceTests()
        {
            _logger = new Mock<NLog.ILogger>();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _userService = new UserService(_logger.Object, _unitOfWork.Object);
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutomapperWebProfile>();
                cfg.AddProfile<MapProfile>();
            });
        }

        [Test]
        public void GetAllUsers_TestReopitory_GetNotNull()
        {
            IEnumerable<User> users = new List<User>
            {
                new User()
            };
            _unitOfWork.Setup(unit => unit.UserRepository.Get())
                .Returns(users);

            var result = _userService.GetAllUsers();

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetAllUsers_TestReopitory_GetNotEmpty()
        {
            IEnumerable<User> users = new List<User>
            {
                new User()
            };
            _unitOfWork.Setup(unit => unit.UserRepository.Get(
                    It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns(users);

            var result = _userService.GetAllUsers();

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetUser_TestReopitory_GetNotNull()
        {
            IEnumerable<User> users = new List<User>
            {
                new User()
            };
            _unitOfWork.Setup(unit => unit.UserRepository
                    .Get(It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns(users);

            var result = _userService.GetUser("Email");

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetUser_TestReopitory_GetWithCorrectName()
        {
            IEnumerable<User> users = new List<User>
            {
                new User
                {
                    Name = "pavel"
                }
            };
            _unitOfWork.Setup(unit => unit.UserRepository
                    .Get(It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns(users);

            var result = _userService.GetUser("Email");

            Assert.AreEqual("pavel", result.Name);
        }

        [Test]
        public void GetUserById_TestReopitory_GetNotNull()
        {
            IEnumerable<User> users = new List<User>
            {
                new User()
            };
            _unitOfWork.Setup(unit => unit.UserRepository
                    .Get(It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns(users);

            var result = _userService.GetUserById(1);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetUserById_TestReopitory_GetWithCorrectId()
        {
            IEnumerable<User> users = new List<User>
            {
                new User
                {
                    Id = 1
                }
            };
            _unitOfWork.Setup(unit => unit.UserRepository
                    .Get(It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<Expression<Func<User, object>>[]>()))
                .Returns(users);

            var result = _userService.GetUser("Email");

            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void Register_TestSave_WorkOnce()
        {
            var userDto = new UserDTO
            {
                Name = "Name",
                Password = "password"
            };
            IEnumerable<Role> roles = new List<Role>
            {
                new Role()
            };
            _unitOfWork.Setup(unit => unit.RoleRepository
                .Get(It.IsAny<Expression<Func<Role, bool>>>(),
                    It.IsAny<Expression<Func<Role, object>>[]>())).Returns(roles);
            _unitOfWork.Setup(unit => unit.UserRepository.Create(It.IsAny<User>()));

            _userService.Register(userDto);

            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void Edit_TestSave_WorksNever()
        {
            var userDto = new UserDTO
            {
                Id = 1,
                Name = "Name"
            };
            IEnumerable<User> users = new List<User>();
            _unitOfWork.Setup(unit => unit.UserRepository
                .Get(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Expression<Func<User, object>>[]>())).Returns(users);

            _userService.Edit(userDto);

            _unitOfWork.Verify(unit => unit.Save(), Times.Never);
        }

        [Test]
        public void Edit_TestSave_WorksOnce()
        {
            var userDto = new UserDTO
            {
                Id = 1,
                Name = "Name",
                RoleIds = new List<int>()
            };
            IEnumerable<User> users = new List<User>
            {
                new User()
            };
            _unitOfWork.Setup(unit => unit.UserRepository
                .Get(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Expression<Func<User, object>>[]>())).Returns(users);
            _unitOfWork.Setup(unit => unit.UserRepository.Update(It.IsAny<User>()));

            _userService.Edit(userDto);

            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void EditSender_TestSave_WorksOnce()
        {
            _unitOfWork.Setup(unit => unit.UserRepository
                .Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User> { new User() });
            _unitOfWork.Setup(unit => unit.UserRepository.Update(It.IsAny<User>()));

            _userService.EditSender("ME", 1);

            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void EditSender_TestSave_WorksNever()
        {
            _unitOfWork.Setup(unit => unit.UserRepository
                .Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>());

            _userService.EditSender("ME", 1);

            _unitOfWork.Verify(unit => unit.Save(), Times.Never);
        }
    }
}