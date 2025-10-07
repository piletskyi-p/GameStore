using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.Bll.Tests
{
    public class CommentServiceTests
    {
        private CommentService _commentService;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ILogger> _logger;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger>();
            _commentService = new CommentService(_logger.Object, _unitOfWork.Object);
            _mapper = new Mock<IMapper>();

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<MapProfile>());
        }

        [Test]
        public void GetAllCommentsByGameKey_GetCommentCountMoreThenZero_ReturnsComments()
        {
            string key = "ME";
            IEnumerable<CommentDTO> resultList = new List<CommentDTO>();
            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Name = "Pasha",
                    Body = "Hello"
                }
            };
            var game = new List<Game>
            {
                new Game()
            };

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository.Get(It.IsAny<Func<Comment, bool>>()))
                .Returns(comments);
            _mapper.Setup(mapper => mapper.Map<List<Comment>, IEnumerable<CommentDTO>>(
                It.IsAny<List<Comment>>())).Returns(resultList);

            resultList = _commentService.GetAllCommentsByGameKey(key);

            Assert.IsNotEmpty(resultList);
        }

        [Test]
        public void GetAllCommentsByGameKey_GetNotNull_ReturnsComments()
        {
            string key = "ME";
            IEnumerable<CommentDTO> resultList = new List<CommentDTO>();
            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Name = "Pasha",
                    Body = "Hello"
                }
            };
            var game = new List<Game>
            {
                new Game()
            };

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository.Get(It.IsAny<Func<Comment, bool>>()))
                .Returns(comments);
            _mapper.Setup(mapper => mapper.Map<List<Comment>, IEnumerable<CommentDTO>>(
                It.IsAny<List<Comment>>())).Returns(resultList);

            resultList = _commentService.GetAllCommentsByGameKey(key);

            Assert.IsNotNull(resultList);
        }

        [Test]
        public void GetAllCommentsByGameKey_GetCorrectValue_ReturnsComments()
        {
            string key = "ME";
            IEnumerable<CommentDTO> resultList = new List<CommentDTO>();
            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Name = "Pasha",
                    Body = "Hello"
                }
            };
            var game = new List<Game>
            {
                new Game()
            };

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository.Get(It.IsAny<Func<Game, bool>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository.Get(It.IsAny<Func<Comment, bool>>()))
                .Returns(comments);
            _mapper.Setup(mapper => mapper.Map<List<Comment>, IEnumerable<CommentDTO>>(
                It.IsAny<List<Comment>>())).Returns(resultList);

            resultList = _commentService.GetAllCommentsByGameKey(key);

            Assert.AreEqual("Pasha", resultList.First().Name);
        }

        [Test]
        public void Delete_testSave_SaveOnce()
        {
            var list = new List<Comment>
            {
                new Comment()
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository.Get(It.IsAny<Func<Comment, bool>>())).Returns(list);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository.Update(It.IsAny<Comment>()));
            _commentService.Delete(1);
            _unitOfWork.Verify(service => service.Save(), Times.Once());
        }
    }
}
