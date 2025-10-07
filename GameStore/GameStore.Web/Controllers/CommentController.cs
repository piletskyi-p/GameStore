using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;
using GameStore.Web.Models.ViewModels;
using NLog;

namespace GameStore.Web.Controllers
{
    public class CommentController : BaseController
    {
        private readonly IGameService _gameService;
        private readonly ICommentService _commentService;
        private readonly IBanService _banService;
        private readonly IUserService _userService;
        private readonly ILanguageService _languageService;

        private readonly ILogger _logger;

        public CommentController(
            IGameService gameService,
            ICommentService commentService,
            IBanService banService,
            ILogger logger,
            IUserService userService,
            IAuthentication auth,
            ILanguageService languageService) : base(auth)
        {
            _gameService = gameService;
            _commentService = commentService;
            _banService = banService;
            _logger = logger;
            _userService = userService;
            _languageService = languageService;
        }

        [HttpPost]
        public ActionResult LeaveCommentForGame(CommentsViewModel newComment)
        {
            ModelState.Clear();
            var game = _gameService.GetAllGames().FirstOrDefault(key => key.Key == newComment.GameKey);

            if (game == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userE = Auth.CurrentUser.Identity.Name;
            if (userE != "anonym")
            {
                var user = _userService.GetUser(userE);
                if (user.IsBanned)
                {
                    ModelState.AddModelError("NewBody", "You are banned!");
                }
            }

            if (ModelState.IsValid)
            {
                var commentDto = new CommentDTO
                {
                    Name = newComment.NewName,
                    Body = newComment.NewBody,
                    GameId = game.Id,
                    ParentId = newComment.CurrentParentId,
                    QuoteId = newComment.CurrentQuoteId
                };

                var userEmail = Auth.CurrentUser.Identity.Name;
                if (userEmail != "anonym")
                {
                    var userId = _userService.GetUser(userEmail).Id;
                    commentDto.UserId = userId;
                }

                _logger.Info($"User {newComment.NewName} left comment to game " +
                             $"with key {game.Key}");

                _commentService.AddCommentToGame(commentDto, game.Key);

                return GetAllCommentsByGameKey(game.Key);
            }

            var comments = _commentService.GetAllCommentsByGameKey(game.Key).ToList();

            var commentsViewModel = comments.Any()
                ? CreateCommentsViewModel(comments)
                : CreateEmptyCommentsViewModel(game.Key);

            commentsViewModel.GameKey = game.Key;
            if (Auth.CurrentUser.Identity.IsAuthenticated)
            {
                var userEmail = Auth.CurrentUser.Identity.Name;
                var user = _userService.GetUser(userEmail);
                commentsViewModel.NewName = user.Name + " " + user.Surname;
            }

            return View("ViewComments", commentsViewModel);
        }

        public List<CommentDTO> SetUsersToComments(string gamekey)
        {
            var comments = _commentService.GetAllCommentsByGameKey(gamekey).ToList();

            // get list where can be null
            var usersWithNull = comments.Select(com => com.UserId)
                .Select(id => _userService.GetUserById(id)).ToList();
            List<UserDTO> users = new List<UserDTO>();

            // get list without null
            foreach (var user in usersWithNull)
            {
                if (user != null)
                {
                    users.Add(user);
                }
            }

            foreach (var comment in comments)
            {
                if (comment.UserId != 0)
                {
                    comment.User = users.FirstOrDefault(user => user.Id == comment.UserId);
                }
            }

            return comments;
        }

        [HttpGet]
        public ActionResult GetAllCommentsByGameKey(string gamekey)
        {
            if (string.IsNullOrEmpty(gamekey))
            {
                ModelState.AddModelError("Key", "Enter game's key");
            }

            var game = _gameService.GetGameByKey(gamekey, CurrentLangCode);
            var comments = SetUsersToComments(gamekey);

            _logger.Info($"Admin got all comments by game key: {gamekey}");

            var commentsViewModel = comments.Any()
                 ? CreateCommentsViewModel(comments)
                 : CreateEmptyCommentsViewModel(gamekey);

            commentsViewModel.GameKey = gamekey;
            if (Auth.CurrentUser.Identity.IsAuthenticated)
            {
                var userEmail = Auth.CurrentUser.Identity.Name;
                var user = _userService.GetUser(userEmail);
                commentsViewModel.NewName = user.Name + " " + user.Surname;
                commentsViewModel.IsDeletedGame = game.IsDeleted;
                commentsViewModel.CurrentUser = Mapper.Map<UserViewModel>(user);
            }
            else
            {
                commentsViewModel.CurrentUser = new UserViewModel();
            }


            return View("ViewComments", commentsViewModel);
        }

        [HttpPost]
        public ActionResult DeleteComment(int commentId, int gameId)
        {
            if (commentId == 0 || gameId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var game = _gameService.GetItemById(gameId, CurrentLangCode);
            _commentService.Delete(commentId);

            return GetAllCommentsByGameKey(game.Key);
        }

        public ActionResult Ban(int commentId, string gamekey)
        {
            var ban = new BanViewModel
            {
                CommentId = commentId,
                GameKey = gamekey
            };

            return View("Ban", ban);
        }

        [HttpPost]
        public ActionResult BanHandler(BanViewModel ban)
        {
            _banService.Ban(ban.CommentId, ban.Period);
            _logger.Info("User was baned");

            return GetAllCommentsByGameKey(ban.GameKey);
        }

        public CommentsViewModel CreateCommentsViewModel(List<CommentDTO> commentList)
        {
            var commentsView = Mapper.Map<List<CommentDTO>, List<CommentForList>>(commentList);

            foreach (var commentV in commentsView)
            {
                if (commentV.ParentId != 0)
                {
                    commentV.ParentName = commentList
                        .FirstOrDefault(c => c.Id == commentV.ParentId)
                        ?.Name;
                }

                if (commentV.QuoteId != 0)
                {
                    commentV.QuoteText = commentList
                        .FirstOrDefault(c => c.Id == commentV.QuoteId)
                        ?.Body;
                }
            }

            var commentView = new CommentsViewModel
            {
                Comments = new List<CommentForList>()
            };
            commentView.Comments.AddRange(commentsView);

            return commentView;
        }

        public CommentsViewModel CreateEmptyCommentsViewModel(string gamekey)
        {
            var game = _gameService.GetGameByKey(gamekey, CurrentLangCode);
            var commentsView = new List<CommentForList>
            {
                new CommentForList
                {
                    GameId = game.Id
                }
            };

            var commentView = new CommentsViewModel
            {
                Comments = new List<CommentForList>()
            };
            commentView.Comments = commentsView;

            return commentView;
        }

        public ActionResult ReturnIfCommentsExist(List<CommentDTO> commentList, GameDTO gameDto)
        {
            _logger.Info($"Admin got all comments by game key: {gameDto.Key}");
            var commentstemp = Mapper.Map<List<CommentDTO>, List<CommentForList>>(commentList);

            foreach (var comment in commentList)
            {
                foreach (var comMv in commentstemp)
                {
                    if (comment.Id == comMv.ParentId)
                    {
                        comMv.ParentName = comment.Name;
                        comMv.ParentComment = comment.Body;
                        break;
                    }
                }
            }

            var commentViewModel = new CommentsViewModel
            {
                Comments = new List<CommentForList>()
            };
            commentViewModel.Comments.AddRange(commentstemp);
            commentViewModel.GameKey = gameDto.Key;
            if (Auth.CurrentUser.Identity.IsAuthenticated)
            {
                var userEmail = Auth.CurrentUser.Identity.Name;
                var user = _userService.GetUser(userEmail);
                commentViewModel.NewName = user.Name + " " + user.Surname;
                commentViewModel.IsDeletedGame = gameDto.IsDeleted;
                commentViewModel.CurrentUser = Mapper.Map<UserViewModel>(user);
            }
            else
            {
                commentViewModel.CurrentUser = new UserViewModel();
            }

            return PartialView("AllCommentsPartial", commentViewModel);
        }

        public ActionResult CreateAndReturnIfCommentsNotExist(GameDTO gameDto)
        {
            var game = _gameService.GetGameByKey(gameDto.Key, CurrentLangCode);

            var commentsView = new List<CommentForList>
            {
                new CommentForList
                {
                    GameId = game.Id
                }
            };

            var commentView = new CommentsViewModel
            {
                Comments = new List<CommentForList>()
            };
            commentView.Comments.AddRange(commentsView);
            commentView.GameKey = gameDto.Key;
            if (Auth.CurrentUser.Identity.IsAuthenticated)
            {
                var userEmail = Auth.CurrentUser.Identity.Name;
                var user = _userService.GetUser(userEmail);
                commentView.NewName = user.Name + " " + user.Surname;
                commentView.IsDeletedGame = gameDto.IsDeleted;
                commentView.CurrentUser = Mapper.Map<UserViewModel>(user);
            }
            else
            {
                commentView.CurrentUser = new UserViewModel();
            }

            return PartialView("AllCommentsPartial", commentView);
        }
    }
}