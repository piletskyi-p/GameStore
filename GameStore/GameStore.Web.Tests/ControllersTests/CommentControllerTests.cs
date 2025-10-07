using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;
using GameStore.Web.Controllers;
using GameStore.Web.Models.ViewModels;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests
{
    public class CommentControllerTests
    {
        private readonly Mock<IGameService> _gameService;
        private readonly Mock<ICommentService> _commentService;
        private readonly Mock<IBanService> _banService;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUserService> _userService;
        private readonly Mock<ILanguageService> _languageService;
        private Mock<IAuthentication> _auth;
        private CommentController _commentController;

        public CommentControllerTests()
        {
            _gameService = new Mock<IGameService>();
            _commentService = new Mock<ICommentService>();
            _banService = new Mock<IBanService>();
            _logger = new Mock<ILogger>();
            _mapper = new Mock<IMapper>();
            _userService = new Mock<IUserService>();
            _languageService = new Mock<ILanguageService>();
        }

        [SetUp]
        public void Setup()
        {
            _auth = new Mock<IAuthentication>();
            _commentController = new CommentController(
                _gameService.Object,
                _commentService.Object,
                _banService.Object,
                _logger.Object,
                _userService.Object,
                _auth.Object,
                _languageService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void GetAllCommentsByGameKey_GetComments_ReturnComments()
        {
            string key = "ME";
            var game = new List<CommentDTO>
            {
                new CommentDTO
                {
                    Id = 1,
                    Name = "Pasha",
                    Body = "hello"
                }
            };

            _mapper.Setup(mapper => mapper.Map<List<CommentDTO>, List<CommentsViewModel>>(
                It.IsAny<List<CommentDTO>>())).Returns(It.IsAny<List<CommentsViewModel>>());
            _commentService.Setup(commentService => commentService.GetAllCommentsByGameKey(It.IsAny<string>()))
                .Returns(game);
            _auth.Setup(action => action.CurrentUser.Identity.IsAuthenticated).Returns(false);

            var result = (ViewResult)_commentController.GetAllCommentsByGameKey(key);

            Assert.IsNotEmpty(result.Model.ToString());
        }

        [Test]
        public void GetAllCommentsByGameKey_CommentsDontExist_ReturnComment()
        {
            var game = new List<CommentDTO>();
            var gameDto = new GameDTO();
            _mapper.Setup(mapper => mapper.Map<List<CommentDTO>, List<CommentsViewModel>>(
                It.IsAny<List<CommentDTO>>())).Returns(It.IsAny<List<CommentsViewModel>>());
            _commentService.Setup(commentService => commentService.GetAllCommentsByGameKey(It.IsAny<string>()))
                .Returns(game);
            _gameService.Setup(g => g.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(gameDto);
            _auth.Setup(action => action.CurrentUser.Identity.IsAuthenticated).Returns(false);

            var result = (ViewResult)_commentController.GetAllCommentsByGameKey("ME");

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void GetAllCommentsByGameKey_CorrectView_ReturnComment()
        {
            var game = new List<CommentDTO>();
            var gameDto = new GameDTO();
            _mapper.Setup(mapper => mapper.Map<List<CommentDTO>, List<CommentsViewModel>>(
                It.IsAny<List<CommentDTO>>())).Returns(It.IsAny<List<CommentsViewModel>>());
            _commentService.Setup(commentService => commentService.GetAllCommentsByGameKey(It.IsAny<string>()))
                .Returns(game);
            _gameService.Setup(g => g.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(gameDto);
            _auth.Setup(action => action.CurrentUser.Identity.IsAuthenticated).Returns(false);

            var result = (ViewResult)_commentController.GetAllCommentsByGameKey("ME");

            Assert.AreEqual("ViewComments", result.ViewName);
        }

        [Test]
        public void Ban_CorrectView_ReturnView()
        {
            var res = (ViewResult)_commentController.Ban(It.IsAny<int>(), It.IsAny<string>());
            Assert.AreEqual("Ban", res.ViewName);
        }

        [Test]
        public void CreateAndReturnIfCommentsNotExist_CorrectView_ReturnView()
        {
            var game = new GameDTO
            {
                Id = 1,
                Key = "ME"
            };
            _gameService.Setup(service => service.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(game);
            _auth.Setup(action => action.CurrentUser.Identity.IsAuthenticated).Returns(false);

            var res = (PartialViewResult)_commentController
                .CreateAndReturnIfCommentsNotExist(new GameDTO());
            Assert.AreEqual("AllCommentsPartial", res.ViewName);
        }

        [Test]
        public void CreateAndReturnIfCommentsNotExist_CorrectModel_ReturnNotNullModel()
        {
            var game = new GameDTO
            {
                Id = 1,
                Key = "ME"
            };
            _gameService.Setup(service => service.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(game);
            _userService.Setup(service => service.GetUser(It.IsAny<string>()))
                .Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.IsAuthenticated)
                .Returns(true);

            var res = (PartialViewResult)_commentController
                .CreateAndReturnIfCommentsNotExist(new GameDTO());
            Assert.IsNotNull(res.Model);
        }

        [Test]
        public void ReturnIfCommentsExist_CorrectModel_ReturnNotNullModel()
        {
            var game = new GameDTO
            {
                Id = 1,
                Key = "ME"
            };
            var list = new List<CommentDTO>();
            _gameService.Setup(service => service.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(game);
            _userService.Setup(service => service.GetUser(It.IsAny<string>()))
                .Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.IsAuthenticated)
                .Returns(true);

            var res = (PartialViewResult)_commentController
                .ReturnIfCommentsExist(list, game);
            Assert.IsNotNull(res.Model);
        }

        [Test]
        public void ReturnIfCommentsExist_CorrectView_ReturnView()
        {
            var game = new GameDTO
            {
                Id = 1,
                Key = "ME"
            };
            var list = new List<CommentDTO>();
            _gameService.Setup(service => service.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(game);
            _userService.Setup(service => service.GetUser(It.IsAny<string>()))
                .Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.IsAuthenticated)
                .Returns(true);

            var res = (PartialViewResult)_commentController
                .CreateAndReturnIfCommentsNotExist(new GameDTO());

            Assert.AreEqual("AllCommentsPartial", res.ViewName);
        }

        [Test]
        public void CreateEmptyCommentsViewModel_Model_ReturnNotNull()
        {
            var game = new GameDTO
            {
                Id = 1,
                Key = "ME"
            };
            var list = new List<CommentDTO>();
            _gameService.Setup(service => service.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(game);

            var res = _commentController
                .CreateEmptyCommentsViewModel("ME");

            Assert.NotNull(res);
        }

        [Test]
        public void CreateCommentsViewModel_Model_ReturnNotNull()
        {
            var game = new GameDTO
            {
                Id = 1,
                Key = "ME"
            };
            var list = new List<CommentDTO>();

            var res = _commentController
                .CreateCommentsViewModel(list);

            Assert.NotNull(res);
        }

        [Test]
        public void Ban_Model_ReturnNotNull()
        {
            var res = (ViewResult)_commentController
                .Ban(1, string.Empty);

            Assert.NotNull(res.Model);
        }
        
        [Test]
        public void Ban_Model_ReturnCorrectView()
        {
            var res = (ViewResult)_commentController
                .Ban(1, string.Empty);

            Assert.AreEqual("Ban", res.ViewName);
        }

        [Test]
        public void LeaveCommentForGame_Model_ReturnError()
        {
            IEnumerable<GameDTO> games = new List<GameDTO>();
            _gameService.Setup(game => game.GetAllGames()).Returns(games);
            var error = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var res = _commentController
                .LeaveCommentForGame(new CommentsViewModel());

            Assert.AreEqual(error.GetType(), res.GetType());
        }

        [Test]
        public void LeaveCommentForGame_Model_ReturnView()
        {
            IEnumerable<GameDTO> games = new List<GameDTO>
            {
                new GameDTO()
            };
            var gameDto = new GameDTO();
            _gameService.Setup(game => game.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(gameDto);
            _gameService.Setup(game => game.GetAllGames()).Returns(games);
            _auth.Setup(auth => auth.CurrentUser.Identity.Name).Returns("anonym");
            var res = (ViewResult)_commentController
                .LeaveCommentForGame(new CommentsViewModel());

            Assert.AreEqual("ViewComments", res.ViewName);
        }

        [Test]
        public void LeaveCommentForGame_Model_ReturnNotNullModel()
        {
            IEnumerable<GameDTO> games = new List<GameDTO>
            {
                new GameDTO()
            };
            var gameDto = new GameDTO();
            _gameService.Setup(game => game.GetGameByKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(gameDto);
            _gameService.Setup(game => game.GetAllGames()).Returns(games);
            _auth.Setup(auth => auth.CurrentUser.Identity.Name).Returns("anonym");
            var res = (ViewResult)_commentController
                .LeaveCommentForGame(new CommentsViewModel());

            Assert.IsNotNull(res.Model);
        }
    }
}