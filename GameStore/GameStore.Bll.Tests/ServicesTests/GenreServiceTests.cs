using System;
using System.Collections.Generic;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using GameStore.Web;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests
{
    public class GenreServiceTests
    {
        private GenreService _genreService;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _genreService = new GenreService(_unitOfWork.Object);
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
            _unitOfWork.Setup(genr => genr.GenreRepository.Get()).Returns(It.IsAny<List<Genre>>());

            var result = _genreService.GetAll();
            Assert.IsNull(result);
        }

        [Test]
        public void GetForGameByKey_GetAllGenres_ReturnEmptylist()
        {
            var game = new List<Game>();
            _unitOfWork.Setup(genr => genr.GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);

            var result = _genreService.GetByGameKey("ME");
            Assert.IsEmpty(result);
        }

        [Test]
        public void Delete_TestSave_SaveOnce()
        {
            _unitOfWork.Setup(platform => platform.GenreRepository.Delete(1));
            _genreService.Delete(1);
            _unitOfWork.Verify(method => method.Save(), Times.Once);
        }

        [Test]
        public void GetById_GetByKey_EmptyList()
        {
            var genre = new List<Genre>();
            _unitOfWork.Setup(genr => genr.GenreRepository
                    .Get(It.IsAny<Func<Genre, bool>>()))
                .Returns(genre);

            var result = _genreService.GetById(1);
            Assert.IsNull(result);
        }

        [Test]
        public void GetById_GetByKey_NotEmpty()
        {
            var genre = new List<Genre>
            {
                new Genre()
            };
            _unitOfWork.Setup(genr => genr.GenreRepository
                    .Get(It.IsAny<Func<Genre, bool>>()))
                .Returns(genre);

            var result = _genreService.GetById(1);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Update_TestSave_SaveOnce()
        {
            _unitOfWork.Setup(genr => genr.GenreRepository
                .Update(It.IsAny<Genre>()));

            _genreService.Update(It.IsAny<GenreDTO>());
            _unitOfWork.Verify(method => method.Save(), Times.Once);
        }
    }
}