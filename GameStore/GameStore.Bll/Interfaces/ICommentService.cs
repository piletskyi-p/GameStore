using System.Collections.Generic;
using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface ICommentService
    {
        void AddCommentToGame(CommentDTO comment, string gamekey);
        IEnumerable<CommentDTO> GetAllCommentsByGameKey(string key);
        IEnumerable<CommentDTO> GetAllDeletedCommentsByGameKey(string key);
        IEnumerable<CommentDTO> GetAllCommentsByGameId(int id);
        CommentDTO GetCommentByGameId(int commentId, int gameId);
        CommentDTO GetCommentById(int id);
        void Delete(int id);
    }
}
