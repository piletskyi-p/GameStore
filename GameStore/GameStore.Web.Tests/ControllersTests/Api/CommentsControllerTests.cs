using System.Collections.Generic;
using System.Net;
using System.Web.Http.Results;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Controllers.Api;
using Moq;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests.Api
{
    public class CommentsControllerTests
    {
        private readonly Mock<ICommentService> _commentService;
        private readonly Mock<IGameService> _gameService;
        private readonly Mock<IUserService> _userService;
        private CommentsController _commentsController;

        public CommentsControllerTests()
        {
            _commentService = new Mock<ICommentService>();
            _gameService = new Mock<IGameService>();
            _userService = new Mock<IUserService>();
        }

        [SetUp]
        public void Setup()
        {
            _commentsController = new CommentsController(
                _commentService.Object,
                _gameService.Object,
                _userService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void GetDetails_GetValue_GetNotFount()
        {
            _commentService.Setup(service => service
                .GetCommentByGameId(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(It.IsAny<CommentDTO>());

            var result = _commentsController.GetDetails(1, 2);
            var contentResult = result as NegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void GetDetails_GetValue_GetComment()
        {
            _commentService.Setup(service => service
                    .GetCommentByGameId(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new CommentDTO());

            var result = _commentsController.GetDetails(1, 2);
            var contentResult = result as NegotiatedContentResult<CommentDTO>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Get_GetValue_GetNotFount()
        {
            _commentService.Setup(service => service
                    .GetAllCommentsByGameId(It.IsAny<int>()))
                .Returns(new List<CommentDTO>());

            var result = _commentsController.Get(1);
            var contentResult = result as NegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Get_GetValue_GetComment()
        {
            IEnumerable<CommentDTO> comments = new List<CommentDTO>
            {
                new CommentDTO()
            };
            _commentService.Setup(service => service
                    .GetAllCommentsByGameId(It.IsAny<int>()))
                .Returns(comments);

            var result = _commentsController.Get(1);
            var contentResult = result as NegotiatedContentResult<List<CommentDTO>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Delete_GetValue_GetNotFount()
        {
            _commentService.Setup(service => service
                .Delete(It.IsAny<int>()));

            var result = _commentsController.Delete(1);
            var contentResult = result as NegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        [Test]
        public void Delete_GetValue_GetComment()
        {
            _commentService.Setup(service => service
                    .GetCommentByGameId(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new CommentDTO());
            _commentService.Setup(service => service.Delete(It.IsAny<int>()));

            var result = _commentsController.Delete(1);
            var contentResult = result as NegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }
    }
}