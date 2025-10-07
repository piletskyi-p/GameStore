using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.DTO.TranslateDto;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Interfaces;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Translate;
using GameStore.Dal.Interfaces;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServiceTests
{
    public class PlatformServiceTests
    {
        private PlatformService _platformService;
        private readonly Mock<IMapper> _mapper;
        private Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IEventLogger> _baseService;

        public PlatformServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _baseService = new Mock<IEventLogger>();
            _platformService = new PlatformService(_unitOfWork.Object, _baseService.Object);
            _mapper = new Mock<IMapper>();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _platformService = new PlatformService(_unitOfWork.Object, _baseService.Object);
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<MapProfile>());
        }

        [Test]
        public void GetAll_GetAllPlatform_ReturnEmptyList()
        {
            _unitOfWork.Setup(genr => genr.PlatformRepository
                .Get(It.IsAny<Expression<Func<Platform, bool>>>(),
                    It.IsAny<Expression<Func<Platform, object>>[]>())).Returns(It.IsAny<List<Platform>>());

            var result = _platformService.GetAll("lang");
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetAll_GetAllPlatform_ReturnNotEmptyList()
        {
            List<Platform> platforms = new List<Platform>
            {
                new Platform
                {
                    PlatformTranslates = new List<PlatformTranslate>()
                }
            };
            List<PlatformDTO> platformsDto = new List<PlatformDTO>
            {
                new PlatformDTO
                {
                    Id = 1,
                    Type = "Bla",
                    Games = new List<GameDTO>(),
                    PlatformTranslates = new List<PlatformTranslateDto>()
                }
            };
            _unitOfWork.Setup(genr => genr.PlatformRepository
                .Get(
                    It.IsAny<Expression<Func<Platform, object>>[]>())).Returns(platforms);
            _mapper.Setup(map => map.Map<List<Platform>, List<PlatformDTO>>(platforms))
                .Returns(platformsDto);

            var result = _platformService.GetAll("lang");
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetByGameKey_GetAllPlatform_ReturnEmptyList()
        {
            IEnumerable<Game> games = new List<Game>();
            _unitOfWork.Setup(genr => genr.GameRepository
                .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                    It.IsAny<Expression<Func<Game, object>>[]>())).Returns(games);

            var result = _platformService.GetByGameKey("ME", "lang");
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetByGameKey_GetAllPlatform_ReturnNotEmptyList()
        {
            IEnumerable<Game> games = new List<Game>
            {
                new Game
                {
                    Platforms = new List<Platform>
                    {
                        new Platform
                        {
                            PlatformTranslates = new List<PlatformTranslate>()
                        }
                    }
                }
            };
            IEnumerable<PlatformDTO> platformDto = new List<PlatformDTO>
            {
                new PlatformDTO
                {
                    Id = 1,
                    Type = "Bla",
                    Games = new List<GameDTO>(),
                    PlatformTranslates = new List<PlatformTranslateDto>()
                }
            };
            _unitOfWork.Setup(genr => genr.GameRepository
                .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                    It.IsAny<Expression<Func<Game, object>>[]>())).Returns(games);
            _mapper.Setup(map => map.Map<IEnumerable<Platform>, IEnumerable<PlatformDTO>>(games.First().Platforms))
                .Returns(platformDto);

            var result = _platformService.GetByGameKey("ME", "lang");
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void Update_TestSave_SaveOnce()
        {
            var platform = new Platform
            {
                Id = 1,
                PlatformTranslates = new List<PlatformTranslate>(),
                Games = new List<Game>()
            };
            _unitOfWork.Setup(genr => genr.PlatformRepository
                .Update(It.IsAny<Platform>()));
            _unitOfWork.Setup(unit => unit.PlatformRepository
                .FindById(It.IsAny<int>(),
                    It.IsAny<Expression<Func<Platform, object>>[]>())).Returns(platform);
            _baseService
                .Setup(service => service.LogUpdate(It.IsAny<Platform>(), It.IsAny<Platform>()));

            _platformService.Update(new PlatformDTO { Id = 1 });
            _unitOfWork.Verify(methodsave => methodsave.Save(), Times.AtMost(2));
        }

        [Test]
        public void Delete_TestSave_SaveOnce()
        {
            _unitOfWork.Setup(platform => platform.PlatformRepository.Delete(1));
            _baseService
                .Setup(service => service.LogDelete(It.IsAny<Platform>()));
            _platformService.Delete(1);
            _unitOfWork.Verify(method => method.Save(), Times.Once);
        }

        [Test]
        public void GetById_GetByKey_EmptyList()
        {
            var platform = new List<Platform>();
            _unitOfWork.Setup(genr => genr.PlatformRepository
                    .Get(It.IsAny<Expression<Func<Platform, bool>>>(),
                        It.IsAny<Expression<Func<Platform, object>>[]>()))
                .Returns(platform);

            var result = _platformService.GetById(1, "lang");
            Assert.IsNull(result);
        }

        [Test]
        public void GetById_GetByKey_NotEmpty()
        {
            var platform = new List<Platform>
            {
                new Platform
                {
                    PlatformTranslates = new List<PlatformTranslate>()
                }
            };
            _unitOfWork.Setup(genr => genr.PlatformRepository
                    .Get(It.IsAny<Expression<Func<Platform, bool>>>(),
                        It.IsAny<Expression<Func<Platform, object>>[]>()))
                .Returns(platform);

            var result = _platformService.GetById(1, "lang");
            Assert.IsNotNull(result);
        }

        [Test]
        public void Create_TestSave_WorkOnce()
        {
            var platform = new PlatformDTO
            {
                PlatformTranslates = new List<PlatformTranslateDto>()
            };
            _unitOfWork.Setup(genr => genr.LanguageRepository
                    .FindById(It.IsAny<int>()))
                .Returns(new Language());
            _unitOfWork.Setup(unit => unit.PlatformRepository.Create(It.IsAny<Platform>()));
            _baseService.Setup(service => service.LogCreate(It.IsAny<Platform>()));

            _platformService.Create(platform);

            _unitOfWork.Verify(method => method.Save(), Times.Once);
        }
    }
}
