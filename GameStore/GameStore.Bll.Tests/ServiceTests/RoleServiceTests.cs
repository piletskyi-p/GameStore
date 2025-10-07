using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Interfaces;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using GameStore.Web;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServiceTests
{
    public class RoleServiceTests
    {
        private readonly Mock<IEventLogger> _eventService;
        private RoleService _roleService;
        private Mock<IUnitOfWork> _unitOfWork;

        public RoleServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _eventService = new Mock<IEventLogger>();
            _roleService = new RoleService(_unitOfWork.Object, _eventService.Object);
        }

        [SetUp]
        public void Setup()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutomapperWebProfile>();
                cfg.AddProfile<MapProfile>();
            });
        }

        [Test]
        public void AddRole_testSeva_WorkOnce()
        {
            var role = new RoleDTO
            {
                Name = "Bla"
            };
            _unitOfWork.Setup(unit => unit.RoleRepository.Create(It.IsAny<Role>()));
            _eventService.Setup(baseservice => baseservice.LogCreate(It.IsAny<Role>()));
            _roleService.AddRole(role);
            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void GetAll_TestGet_GetNotNull()
        {
            IEnumerable<Role> roles = new List<Role>
            {
                new Role()
            };

            _unitOfWork.Setup(unit => unit.RoleRepository.Get()).Returns(roles);

            var result = _roleService.GetAll();

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetAll_TestGet_GetNotEmpty()
        {
            IEnumerable<Role> roles = new List<Role>
            {
                new Role()
            };

            _unitOfWork.Setup(unit => unit.RoleRepository
                .Get(It.IsAny<Expression<Func<Role, object>>[]>())).Returns(roles);

            var result = _roleService.GetAll();

            Assert.IsNotEmpty(result);
        }
    }
}