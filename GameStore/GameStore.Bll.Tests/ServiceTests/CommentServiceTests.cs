using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Interfaces;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServiceTests
{
    public class CommentServiceTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IMapper> _mapper;
        private CommentService _commentService;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IEventLogger> _baseService;

        public CommentServiceTests()
        {
            _logger = new Mock<ILogger>();
            _mapper = new Mock<IMapper>();
            _baseService = new Mock<IEventLogger>();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _commentService = new CommentService(_logger.Object, _unitOfWork.Object, _baseService.Object);
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

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .GetFromAll(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
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

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
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

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .GetFromAll(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(comments);
            _mapper.Setup(mapper => mapper.Map<List<Comment>, IEnumerable<CommentDTO>>(
                It.IsAny<List<Comment>>())).Returns(resultList);

            resultList = _commentService.GetAllCommentsByGameKey(key);

            Assert.AreEqual("Pasha", resultList.First().Name);
        }


        [Test]
        public void GetAllCommentsByGameId_GetCommentCountMoreThenZero_ReturnsComments()
        {
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

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(comments);
            _mapper.Setup(mapper => mapper.Map<List<Comment>, IEnumerable<CommentDTO>>(
                It.IsAny<List<Comment>>())).Returns(resultList);

            resultList = _commentService.GetAllCommentsByGameId(1);

            Assert.IsNotEmpty(resultList);
        }

        [Test]
        public void GetAllCommentsByGameId_GetNotNull_ReturnsComments()
        {
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

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(comments);
            _mapper.Setup(mapper => mapper.Map<List<Comment>, IEnumerable<CommentDTO>>(
                It.IsAny<List<Comment>>())).Returns(resultList);

            resultList = _commentService.GetAllCommentsByGameId(1);

            Assert.IsNotNull(resultList);
        }

        [Test]
        public void GetAllCommentsByGameId_GetCorrectValue_ReturnsComments()
        {
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

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(comments);
            _mapper.Setup(mapper => mapper.Map<List<Comment>, IEnumerable<CommentDTO>>(
                It.IsAny<List<Comment>>())).Returns(resultList);

            resultList = _commentService.GetAllCommentsByGameId(1);

            Assert.AreEqual("Pasha", resultList.First().Name);
        }

        [Test]
        public void GetAllDeletedCommentsByGameKey_GetCommentCountMoreThenZero_ReturnsComments()
        {
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

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .GetDeleted(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(comments);
            _mapper.Setup(mapper => mapper.Map<List<Comment>, IEnumerable<CommentDTO>>(
                It.IsAny<List<Comment>>())).Returns(resultList);

            resultList = _commentService.GetAllDeletedCommentsByGameKey("ME");

            Assert.IsNotEmpty(resultList);
        }

        [Test]
        public void GetAllDeletedCommentsByGameKey_GetNotNull_ReturnsComments()
        {
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

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .GetDeleted(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(comments);
            _mapper.Setup(mapper => mapper.Map<List<Comment>, IEnumerable<CommentDTO>>(
                It.IsAny<List<Comment>>())).Returns(resultList);

            resultList = _commentService.GetAllDeletedCommentsByGameKey("ME");

            Assert.IsNotNull(resultList);
        }

        [Test]
        public void GetAllDeletedCommentsByGameKey_GetCorrectValue_ReturnsComments()
        {
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

            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                    .GetDeleted(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(game);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(comments);
            _mapper.Setup(mapper => mapper.Map<List<Comment>, IEnumerable<CommentDTO>>(
                It.IsAny<List<Comment>>())).Returns(resultList);

            resultList = _commentService.GetAllDeletedCommentsByGameKey("ME");

            Assert.AreEqual("Pasha", resultList.First().Name);
        }


        [Test]
        public void AddCommentToGame_testSave_SaveNever()
        {
            IEnumerable<Game> games = new List<Game>();
            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                .Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(games);
            _commentService.AddCommentToGame(It.IsAny<CommentDTO>(), string.Empty);
            _unitOfWork.Verify(service => service.Save(), Times.Never);
        }

        [Test]
        public void AddCommentToGame_testSave_SaveOnce()
        {
            IEnumerable<Game> games = new List<Game>
            {
                new Game
                {
                    Comments = new List<Comment>()
                }
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork.GameRepository
                .Get(It.IsAny<Expression<Func<Game, bool>>>(), 
                It.IsAny<Expression<Func<Game, object>>[]>())).Returns(games);

            _baseService
                .Setup(service => service.LogCreate(It.IsAny<Comment>()));

            _commentService.AddCommentToGame(It.IsAny<CommentDTO>(), string.Empty);
            _unitOfWork.Verify(service => service.Save(), Times.Once);
        }

        [Test]
        public void Delete_testSave_SaveNever()
        {
            IEnumerable<Comment> comments = new List<Comment>();
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository
                .GetFromAll(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(comments);
            _commentService.Delete(1);
            _unitOfWork.Verify(service => service.Save(), Times.Never);
        }

        [Test]
        public void Delete_testSave_SaveOnce()
        {
            var list = new List<Comment>
            {
                new Comment()
            };
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository
                .GetFromAll(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(list);
            _unitOfWork.Setup(unitOfWork => unitOfWork.CommentRepository.Update(It.IsAny<Comment>()));
            _baseService
                .Setup(service => service.LogCreate(It.IsAny<Comment>()));

            _commentService.Delete(1);
            _unitOfWork.Verify(service => service.Save(), Times.Once());
        }

        [Test]
        public void GetCommentById_GetComments_ReturnsNull()
        {
            IEnumerable<Comment> comments = new List<Comment>();
            _unitOfWork.Setup(unit => unit.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(comments);
            var result = _commentService.GetCommentById(1);

            Assert.IsNull(result);
        }

        [Test]
        public void GetCommentById_GetComments_ReturnsCorrectType()
        {
            IEnumerable<Comment> comments = new List<Comment>
            {
                new Comment()
            };
            _unitOfWork.Setup(unit => unit.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(comments);
            var result = _commentService.GetCommentById(1);

            Assert.AreEqual(typeof(CommentDTO), result.GetType());
        }

        [Test]
        public void GetCommentByGameId_GetComments_ReturnNull()
        {
            _unitOfWork.Setup(unit => unit.CommentRepository
                .FindById(It.IsAny<int>())).Returns(It.IsAny<Comment>());

            var result = _commentService.GetCommentByGameId(1, 1);

            Assert.IsNull(result);
        }

        [Test]
        public void GetCommentByGameId_GetComments_ReturnNotNull()
        {
            _unitOfWork.Setup(unit => unit.CommentRepository
                .FindById(It.IsAny<int>())).Returns(new Comment{GameId = 1});

            var result = _commentService.GetCommentByGameId(1, 1);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetAllCommentsByGameKey_GetComments_ReturnEmptyList()
        {
            _unitOfWork.Setup(unit => unit.GameRepository
                .GetFromAll(It.IsAny<Expression<Func<Game,bool>>>()))
                .Returns(new List<Game>());

            var result = _commentService.GetAllCommentsByGameKey("Me");

            Assert.IsEmpty(result);
        }

        [Test]
        public void GetAllCommentsByGameKey_GetComments_ReturnList()
        {
            _unitOfWork.Setup(unit => unit.GameRepository
                    .GetFromAll(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game>{new Game()});
            _unitOfWork.Setup(unit => unit.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(new List<Comment> {new Comment()});

            var result = _commentService.GetAllCommentsByGameKey("Me");

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetAllCommentsByGameId_GetComments_ReturnEmptyList()
        {
            _unitOfWork.Setup(unit => unit.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game>());
            var result = _commentService.GetAllCommentsByGameId(1);

            Assert.IsEmpty(result);
        }

        [Test]
        public void GetAllCommentsByGameId_GetCommentsAnotherTest_ReturnEmptyList()
        {
            _unitOfWork.Setup(unit => unit.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game>{new Game()});
            _unitOfWork.Setup(unit => unit.CommentRepository
                .Get(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(new List<Comment>());

            var result = _commentService.GetAllCommentsByGameId(1);

            Assert.IsEmpty(result);
        }

        [Test]
        public void GetAllCommentsByGameId_GetComments_ReturnList()
        {
            _unitOfWork.Setup(unit => unit.GameRepository
                    .Get(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game> { new Game() });
            _unitOfWork.Setup(unit => unit.CommentRepository
                .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(new List<Comment>{new Comment()});

            var result = _commentService.GetAllCommentsByGameId(1);

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetAllDeletedCommentsByGameKey_GetComments_ReturnEmptyList()
        {
            _unitOfWork.Setup(unit => unit.GameRepository
                    .GetDeleted(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game>());
            var result = _commentService.GetAllDeletedCommentsByGameKey("ME");

            Assert.IsEmpty(result);
        }

        [Test]
        public void GetAllDeletedCommentsByGameKey_GetCommentsAnotherTest_ReturnEmptyList()
        {
            _unitOfWork.Setup(unit => unit.GameRepository
                    .GetDeleted(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game> { new Game() });
            _unitOfWork.Setup(unit => unit.CommentRepository
                .Get(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(new List<Comment>());

            var result = _commentService.GetAllDeletedCommentsByGameKey("ME");

            Assert.IsEmpty(result);
        }

        [Test]
        public void GetAllDeletedCommentsByGameKey_GetComments_ReturnList()
        {
            _unitOfWork.Setup(unit => unit.GameRepository
                    .GetDeleted(It.IsAny<Expression<Func<Game, bool>>>()))
                .Returns(new List<Game> { new Game() });
            _unitOfWork.Setup(unit => unit.CommentRepository
                    .Get(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns(new List<Comment> { new Comment() });

            var result = _commentService.GetAllDeletedCommentsByGameKey("ME");

            Assert.IsNotEmpty(result);
        }
    }
}
