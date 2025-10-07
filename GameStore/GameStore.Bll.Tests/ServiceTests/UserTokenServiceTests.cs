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
    public class UserTokenServiceTests
    {
        private readonly Mock<NLog.ILogger> _logger;
        private Mock<IUnitOfWork> _unitOfWork;
        private UserTokenService _userTokenService;

        public UserTokenServiceTests()
        {
            _logger = new Mock<NLog.ILogger>();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _userTokenService = new UserTokenService(_logger.Object, _unitOfWork.Object);
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutomapperWebProfile>();
                cfg.AddProfile<MapProfile>();
            });
        }

        [Test]
        public void Create_TestSave_WorksOnce()
        {
            _unitOfWork.Setup(unit => unit.UserTokenRepository
                .Create(It.IsAny<UserToken>()));
            _userTokenService.Create(new UserTokenDto());

            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void GetById_GetToken_GetValue()
        {
            _unitOfWork.Setup(unit => unit.UserTokenRepository
                .FindById(It.IsAny<int>())).Returns(new UserToken());

            var result = _userTokenService.GetById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(UserTokenDto), result.GetType());
        }

        [Test]
        public void GetByToken_GetToken_GetValue()
        {
            IEnumerable<UserToken> tokens = new List<UserToken>
            {
                new UserToken
                {
                    DropDateTime = DateTime.UtcNow.AddDays(2)
                }
            };
            _unitOfWork.Setup(unit => unit.UserTokenRepository
                .Get(It.IsAny<Expression<Func<UserToken, bool>>>())).Returns(tokens);

            var result = _userTokenService.GetByToken("ME");

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(UserTokenDto), result.GetType());
        }

        [Test]
        public void GetByToken_GetToken_GetNull()
        {
            IEnumerable<UserToken> tokens = new List<UserToken>();
            _unitOfWork.Setup(unit => unit.UserTokenRepository
                .Get(It.IsAny<Expression<Func<UserToken, bool>>>())).Returns(tokens);

            var result = _userTokenService.GetByToken("ME");

            Assert.IsNull(result);
        }

        [Test]
        public void GetByToken_TestSave_WorksOnce()
        {
            IEnumerable<UserToken> tokens = new List<UserToken>
            {
                new UserToken
                {
                    DropDateTime = DateTime.UtcNow.AddDays(-2)
                }
            };
            _unitOfWork.Setup(unit => unit.UserTokenRepository
                .Get(It.IsAny<Expression<Func<UserToken, bool>>>())).Returns(tokens);
            _unitOfWork.Setup(unit => unit.UserTokenRepository.Delete(It.IsAny<int>()));

            var result = _userTokenService.GetByToken("ME");

           _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void GetByUserId_GetToken_GetValueNull()
        {
            IEnumerable<UserToken> tokens = new List<UserToken>();
            _unitOfWork.Setup(unit => unit.UserTokenRepository
                .Get(It.IsAny<Expression<Func<UserToken, bool>>>())).Returns(tokens);

            var result = _userTokenService.GetByUserId(1);

            Assert.IsNull(result);
        }

        [Test]
        public void GetByUserId_GetToken_GetValue()
        {
            IEnumerable<UserToken> tokens = new List<UserToken>
            {
                new UserToken()
            };
            _unitOfWork.Setup(unit => unit.UserTokenRepository
                .Get(It.IsAny<Expression<Func<UserToken, bool>>>())).Returns(tokens);

            var result = _userTokenService.GetByUserId(1);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetUserIdByToken_GetToken_GetValue()
        {
            IEnumerable<UserToken> tokens = new List<UserToken>
            {
                new UserToken
                {
                    UserId = 1,
                    DropDateTime = DateTime.UtcNow.AddDays(2)
                }
            };
            _unitOfWork.Setup(unit => unit.UserTokenRepository
                .Get(It.IsAny<Expression<Func<UserToken, bool>>>())).Returns(tokens);

            var result = _userTokenService.GetUserIdByToken("ME");

            Assert.AreNotEqual(0, result);
        }

        [Test]
        public void GetUserIdByToken_TestSave_WorksOnce()
        {
            IEnumerable<UserToken> tokens = new List<UserToken>
            {
                new UserToken
                {
                    UserId = 1,
                    DropDateTime = DateTime.UtcNow.AddDays(-2)
                }
            };
            _unitOfWork.Setup(unit => unit.UserTokenRepository
                .Get(It.IsAny<Expression<Func<UserToken, bool>>>())).Returns(tokens);
            _unitOfWork.Setup(unit => unit.UserTokenRepository.Delete(It.IsAny<int>()));

            var result = _userTokenService.GetUserIdByToken("ME");

            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void GetUserIdByToken_GetToken_GetZero()
        {
            IEnumerable<UserToken> tokens = new List<UserToken>();
            _unitOfWork.Setup(unit => unit.UserTokenRepository
                .Get(It.IsAny<Expression<Func<UserToken, bool>>>())).Returns(tokens);

            var result = _userTokenService.GetUserIdByToken("ME");

            Assert.AreEqual(0, result);
        }
    }
}