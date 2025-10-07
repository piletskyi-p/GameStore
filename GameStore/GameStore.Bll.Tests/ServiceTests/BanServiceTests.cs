using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
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
    public class BanServiceTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IEventLogger> _eventService;
        private BanService _banService;
        private Mock<IUnitOfWork> _unitOfWork;

        public BanServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _eventService = new Mock<IEventLogger>();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _banService = new BanService(_unitOfWork.Object, _eventService.Object);
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutomapperWebProfile>();
                cfg.AddProfile<MapProfile>();
            });
        }

        [Test]
        public void Ban_TestSave_WorksNever()
        {
            IEnumerable<Comment> commets = new List<Comment>();
            IEnumerable<User> users = new List<User>();
            _unitOfWork.Setup(unit => unit.CommentRepository
                .GetFromAll(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(commets);
            _unitOfWork.Setup(unit => unit.UserRepository
                .GetFromAll(It.IsAny<Expression<Func<User, bool>>>())).Returns(users);

            _banService.Ban(1, string.Empty);

            _unitOfWork.Verify(unit => unit.Save(), Times.Never);
        }

        [Test]
        public void Ban_TestSave_WorksOnce()
        {
            IEnumerable<Comment> commets = new List<Comment>();
            IEnumerable<User> users = new List<User>
            {
                new User()
            };
            _unitOfWork.Setup(unit => unit.CommentRepository
                .GetFromAll(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(commets);
            _unitOfWork.Setup(unit => unit.UserRepository
                .GetFromAll(It.IsAny<Expression<Func<User, bool>>>())).Returns(users);
            _unitOfWork.Setup(unit => unit.UserRepository.Update(It.IsAny<User>()));
            _eventService.Setup(service => service.LogUpdate(It.IsAny<User>(), It.IsAny<User>()));

            _banService.Ban(1, string.Empty);

            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void UnBan_TestSave_WorksNever()
        {
            IEnumerable<User> users = new List<User>();
            _unitOfWork.Setup(unit => unit.UserRepository
                .GetFromAll(It.IsAny<Expression<Func<User, bool>>>())).Returns(users);

            _banService.UnBan(1);

            _unitOfWork.Verify(unit => unit.Save(), Times.Never);
        }

        [Test]
        public void UnBan_TestSave_WorksOnce()
        {
            IEnumerable<User> users = new List<User>
            {
                new User()
            };
            _unitOfWork.Setup(unit => unit.UserRepository
                .GetFromAll(It.IsAny<Expression<Func<User, bool>>>())).Returns(users);
            _unitOfWork.Setup(unit => unit.UserRepository.Update(It.IsAny<User>()));

            _banService.UnBan(1);

            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }
    }
}