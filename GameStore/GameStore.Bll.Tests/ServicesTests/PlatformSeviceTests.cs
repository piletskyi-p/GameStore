using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using GameStore.Web;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServicesTests
{
    public class PlatformSeviceTests
    {
        private PlatformService _platformService;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _platformService = new PlatformService(_unitOfWork.Object);
            _mapper = new Mock<IMapper>();

            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutomapperWebProfile>();
                cfg.AddProfile<MapProfile>();
            });
        }

        [Test]
        public void GetAll_GetAllGenres_ReturnNull()
        {
            _unitOfWork.Setup(genr => genr.PlatformRepository
                .Get()).Returns(It.IsAny<List<Platform>>());

            var result = _platformService.GetAll();
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetForGameByKey_GetAllGenres_ReturnEmptylist()
        {
            var game = new List<Game>();
            _unitOfWork.Setup(genr => genr.GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);

            var result = _platformService.GetByGameKey("ME");
            Assert.IsEmpty(result);
        }

        [Test]
        public void Delete_TestSave_SaveOnce()
        {
            _unitOfWork.Setup(platform => platform.PlatformRepository.Delete(1));
            _platformService.Delete(1);
            _unitOfWork.Verify(method => method.Save(), Times.Once);
        }

        [Test]
        public void GetById_GetByKey_EmptyList()
        {
            var platform = new List<Platform>();
            _unitOfWork.Setup(genr => genr.PlatformRepository
                    .Get(It.IsAny<Func<Platform, bool>>()))
                .Returns(platform);

            var result = _platformService.GetById(1);
            Assert.IsNull(result);
        }

        [Test]
        public void GetById_GetByKey_NotEmpty()
        {
            var platform = new List<Platform>
            {
                new Platform()
            };
            _unitOfWork.Setup(genr => genr.PlatformRepository
                    .Get(It.IsAny<Func<Platform, bool>>()))
                .Returns(platform);

            var result = _platformService.GetById(1);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Update_TestSave_SaveOnce()
        {
            _unitOfWork.Setup(genr => genr.PlatformRepository
                .Update(It.IsAny<Platform>()));

            _platformService.Update(It.IsAny<PlatformDTO>());
            _unitOfWork.Verify(method => method.Save(), Times.Once);
        }
    }
}
