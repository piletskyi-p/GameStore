using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;
using GameStore.Web.Controllers;
using GameStore.Web.Models.ViewModels;
using Moq;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests
{
    public class RoleControllerTests
    {
        private readonly Mock<IRoleService> _roleService;
        private readonly Mock<IAuthentication> _auth;
        private RoleController _roleController;

        public RoleControllerTests()
        {
            _roleService = new Mock<IRoleService>();
            _auth = new Mock<IAuthentication>();
        }

        [SetUp]
        public void Setup()
        {
            _roleController = new RoleController(_roleService.Object, _auth.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void AllRoles_TestView_ReturnCorrectView()
        {
            _roleService.Setup(role => role.GetAll())
                .Returns(It.IsAny<IEnumerable<RoleDTO>>());

            var result = (ViewResult)_roleController.AllRoles();

            Assert.AreEqual("AllRoles", result.ViewName);
        }

        [Test]
        public void AllRoles_TestModel_ReturnNotNull()
        {
            IEnumerable<RoleDTO> roles = new List<RoleDTO>
            {
                new RoleDTO()
            };
            _roleService.Setup(role => role.GetAll())
                .Returns(roles);

            var result = (ViewResult)_roleController.AllRoles();

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void NewRole_TestView_ReturnCorrectView()
        {
            var result = (ViewResult)_roleController.NewRole();

            Assert.AreEqual("NewRole", result.ViewName);
        }

        [Test]
        public void NewRole_TestModel_ReturnNotNull()
        {
            var result = (ViewResult)_roleController.NewRole();

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void NewRolePost_TestView_ReturnCorrectView()
        {
            var role = new RoleViewModel
            {
                Name = "Role"
            };
            var result = (ViewResult)_roleController.NewRole(role);

            Assert.AreEqual("AllRoles", result.ViewName);
        }

        [Test]
        public void NewRolePost_TestModel_ReturnNotNull()
        {
            var role = new RoleViewModel
            {
                Name = "Role"
            };
            IEnumerable<RoleDTO> roles = new List<RoleDTO>
            {
                new RoleDTO()
            };
            _roleService.Setup(r => r.GetAll())
                .Returns(roles);

            var result = (ViewResult)_roleController.NewRole(role);

            Assert.IsNotNull(result.Model);
        }
    }
}