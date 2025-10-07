using System;
using System.Collections.Generic;
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
using GameStore.Web;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServiceTests
{
    public class GenreServiceTests
    {
        private readonly Mock<IEventLogger> _baseService;
        private GenreService _genreService;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;

        public GenreServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _baseService = new Mock<IEventLogger>();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _genreService = new GenreService(_unitOfWork.Object, _baseService.Object);
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
            _unitOfWork.Setup(genr => genr.GenreRepository.Get()).Returns(new List<Genre>());

            var result = _genreService.GetAll("dsf");
            Assert.IsNull(result);
        }

        [Test]
        public void GetAll_GetAllGenres_ReturnNotEmpty()
        {
            var genres = new List<Genre>
            {
                new Genre
                {
                    GenreTranslates = new List<GenreTranslate>()
                }
            };
            _unitOfWork.Setup(genr => genr.GenreRepository
                .Get(
                It.IsAny<Expression<Func<Genre, object>>[]>())).Returns(genres);

            var result = _genreService.GetAll("dsf");
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetForGameByKey_GetAllGenres_ReturnEmptylist()
        {
            var game = new List<Game>();
            _unitOfWork.Setup(genr => genr.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(game);

            var result = _genreService.GetByGameKey("ME", "fe");
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetForGameByKey_GetAllGenres_ReturnNotEmptylist()
        {
            var game = new List<Game>
            {
                new Game
                {
                    GameTranslates = new List<GameTranslate>(),
                    Genres = new List<Genre>
                    {
                        new Genre
                        {
                            GenreTranslates = new List<GenreTranslate>
                            {
                                new GenreTranslate
                                {
                                    Language = new Language()
                                }
                            }
                        }
                    }
                }
            };
            _unitOfWork.Setup(genr => genr.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                        It.IsAny<Expression<Func<Game, object>>[]>()))
                .Returns(game);

            var result = _genreService.GetByGameKey("ME", "fe");
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void Delete_TestSave_SaveOnce()
        {
            var genre = new Genre();
            _unitOfWork.Setup(platform => platform.GenreRepository.Delete(1));
            _baseService
                .Setup(service => service.LogDelete(It.IsAny<Genre>()));
            _unitOfWork.Setup(unit => unit.GenreRepository.FindById(It.IsAny<int>())).Returns(genre);

            _genreService.Delete(1);
            _unitOfWork.Verify(method => method.Save(), Times.Once);
        }

        [Test]
        public void GetById_GetByKey_EmptyList()
        {
            var genre = new List<Genre>();

            _unitOfWork.Setup(genr => genr.GenreRepository
                    .Get(It.IsAny<Expression<Func<Genre, bool>>>(),
                        It.IsAny<Expression<Func<Genre, object>>[]>()))
                .Returns(genre);

            var result = _genreService.GetById(1, "lang");
            Assert.IsNull(result);
        }

        [Test]
        public void GetById_GetByKey_NotEmpty()
        {
            var genre = new List<Genre>
            {
                new Genre
                {
                    GenreTranslates = new List<GenreTranslate>()
                }
            };
            _unitOfWork.Setup(genr => genr.GenreRepository
                    .Get(It.IsAny<Expression<Func<Genre, bool>>>(),
                        It.IsAny<Expression<Func<Genre, object>>[]>()))
                .Returns(genre);

            var result = _genreService.GetById(1, "lang");
            Assert.IsNotNull(result);
        }

        [Test]
        public void Update_TestSave_SaveOnce()
        {
            var genre = new Genre
            {
                Id = 1,
                GenreTranslates = new List<GenreTranslate>(),
                ParentId = 1,
                Games = new List<Game>()
            };
            _unitOfWork.Setup(genr => genr.GenreRepository
                .Update(It.IsAny<Genre>()));
            _unitOfWork
                .Setup(unit => unit.GenreRepository
                    .FindById(It.IsAny<int>(),
                        It.IsAny<Expression<Func<Genre, object>>[]>())).Returns(genre);
            _baseService
                .Setup(service => service.LogUpdate(It.IsAny<Genre>(), It.IsAny<Genre>()));

            _genreService.Update(new GenreDTO { Id = 1 });
            _unitOfWork.Verify(method => method.Save(), Times.Once);
        }

        [Test]
        public void Create_TestSave_WorksOnce()
        {
            var genre = new GenreDTO
            {
                GenreTranslates = new List<GenreTranslateDto>()
            };
            _unitOfWork.Setup(genr => genr.GenreRepository.Create(It.IsAny<Genre>()));
            _baseService.Setup(service => service.LogCreate(It.IsAny<Genre>()));
            _genreService.Create(genre);

            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }
    }
}