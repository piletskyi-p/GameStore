using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using NLog;

namespace GameStore.Bll.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventLogger _eventLogger;
        private readonly ILogger _logger;

        public CommentService(ILogger logger, IUnitOfWork unitOfWork, IEventLogger eventLogger)
        {
            _eventLogger = eventLogger;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void AddCommentToGame(CommentDTO commentDto, string gamekey)
        {
            var game = _unitOfWork.GameRepository.Get(
                    gameRepository => gameRepository.Key == gamekey,
                    gameRepository => gameRepository.Comments,
                    gameRepository => gameRepository.Genres,
                    gameRepository => gameRepository.Platforms,
                    gameRepository => gameRepository.GameTranslates)
                .FirstOrDefault();

            if (game != null)
            {
                var comment = Mapper.Map<CommentDTO, Comment>(commentDto);
                game.Comments.Add(comment);
                _unitOfWork.GameRepository.Update(game);
                _unitOfWork.Save();

                _eventLogger.LogCreate(comment);

                _logger.Debug($"(Class: CommentGameService) The method \"AddCommentToGame(CommentDTO " +
                             $"commentDTO, string gamekey)\" worked.");
            }
        }

        public void Delete(int id)
        {
            var comment = _unitOfWork.CommentRepository.GetFromAll(
                commentRepository => commentRepository.Id == id)
                .FirstOrDefault();

            if (comment != null)
            {
                _unitOfWork.CommentRepository.Delete(comment.Id);
                _unitOfWork.Save();

                _eventLogger.LogDelete(comment);
            }
        }

        public IEnumerable<CommentDTO> GetAllCommentsByGameId(int id)
        {
            var game = _unitOfWork.GameRepository
                .Get(gameRepository => gameRepository.Id == id).FirstOrDefault();

            if (game != null)
            {
                var comments = _unitOfWork.CommentRepository.Get(
                    commentRepository => commentRepository.GameId == id).ToList();

                if (comments.Any())
                {
                    var commentDto = Mapper.Map<List<Comment>, List<CommentDTO>>(comments);
                    _logger.Debug(
                        $"(Class: CommentService) The method \"GetAllCommentsByGameKey(string key)\" worked.");

                    return commentDto;
                }
            }

            return new List<CommentDTO>();
        }

        public IEnumerable<CommentDTO> GetAllCommentsByGameKey(string key)
        {
            var game = _unitOfWork.GameRepository.GetFromAll(gameRepository => gameRepository.Key == key).FirstOrDefault();

            if (game != null)
            {
                var comments = _unitOfWork.CommentRepository.Get(
                    commentRepository => commentRepository.GameId == game.Id).ToList();
                
                if (comments.Any())
                {
                    var commentDto = Mapper.Map<List<Comment>, List<CommentDTO>>(comments);
                    _logger.Debug(
                        $"(Class: CommentService) The method \"GetAllCommentsByGameKey(string key)\" worked.");

                    return commentDto;
                }
            }

            return new List<CommentDTO>();
        }

        public IEnumerable<CommentDTO> GetAllDeletedCommentsByGameKey(string key)
        {
            var game = _unitOfWork.GameRepository
                .GetDeleted(gameRepository => gameRepository.Key == key).FirstOrDefault();

            if (game != null)
            {
                var comments = _unitOfWork.CommentRepository.Get(
                    commentRepository => commentRepository.GameId == game.Id).ToList();

                if (comments.Any())
                {
                    var commentDto = Mapper.Map<List<Comment>, List<CommentDTO>>(comments);
                    _logger.Debug(
                        $"(Class: CommentService) The method \"GetAllCommentsByGameKey(string key)\" worked.");

                    return commentDto;
                }
            }

            return new List<CommentDTO>();
        }

        public CommentDTO GetCommentByGameId(int commentId, int gameId)
        {
            var comment = _unitOfWork.CommentRepository.FindById(commentId);
            if (comment != null)
            {
                if (comment.GameId == gameId)
                {
                    return Mapper.Map<CommentDTO>(comment);
                }
            }

            return null;
        }

        public CommentDTO GetCommentById(int id)
        {
            var comments = _unitOfWork.CommentRepository.Get(
                commentRepository => commentRepository.Id == id).FirstOrDefault();

            if (comments != null)
            {
                var commentDto = Mapper.Map<Comment, CommentDTO>(comments);
                _logger.Debug(
                    "(Class: CommentService) The method \"GetAllCommentsByGameKey(string key)\" worked.");

                return commentDto;
            }

            return null;
        }
    }
}
