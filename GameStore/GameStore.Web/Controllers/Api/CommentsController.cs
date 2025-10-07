using System.Linq;
using System.Net;
using System.Web.Http;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Models.ViewModels;

namespace GameStore.Web.Controllers.Api
{
    public class CommentsController : BaseApiController
    {
        private readonly ICommentService _commentService;
        private readonly IGameService _gameService;
        private readonly IUserService _userService;

        public CommentsController(ICommentService commentService, IGameService gameService, IUserService userService)
        {
            _gameService = gameService;
            _commentService = commentService;
            _userService = userService;
        }

        public IHttpActionResult GetDetails(int gameId, int commentId)
        {
            var comment = _commentService.GetCommentByGameId(commentId, gameId);
            if (comment == null)
            {
                return Content(HttpStatusCode.NotFound, "This comment does not exist.");
            }

            return Content(HttpStatusCode.OK, comment);
        }

        public IHttpActionResult Get(int gameId)
        {
            var comments = _commentService.GetAllCommentsByGameId(gameId).ToList();

            if (!comments.Any())
            {
                return Content(HttpStatusCode.NotFound, "This game does not have comments.");
            }

            return Content(HttpStatusCode.OK, comments);
        }

        public IHttpActionResult Post(int gameId, [FromBody]CommentsViewModel value)
        {
            var game = _gameService.GetItemById(gameId, CurrentLangCode);
            if (game == null)
            {
                return Content(HttpStatusCode.BadRequest, "Game does not exist.");
            }

            if (ModelState.IsValid)
            {
                var commentDto = new CommentDTO
                {
                    Name = value.NewName,
                    Body = value.NewBody,
                    GameId = game.Id,
                    ParentId = value.CurrentParentId,
                    QuoteId = value.CurrentQuoteId
                };

                var userEmail = User.Identity.Name;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    var userId = _userService.GetUser(userEmail);
                    commentDto.UserId = userId.Id;
                }

                _commentService.AddCommentToGame(commentDto, game.Key);

                return Content(HttpStatusCode.OK, "Comment was added.");
            }

            return Content(HttpStatusCode.BadRequest, "Game does not exist.");
        }

        [Authorize(Roles = "Moderator")]
        public IHttpActionResult Delete(int commentId)
        {
            if (commentId != 0)
            {
                _commentService.Delete(commentId);

                return Content(HttpStatusCode.OK, "Comment was deleted");
            }

            return BadRequest();
        }
    }
}