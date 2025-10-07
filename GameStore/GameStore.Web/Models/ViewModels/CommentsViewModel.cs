using System.Collections.Generic;

namespace GameStore.Web.Models.ViewModels
{
    public class CommentsViewModel
    {
        public List<CommentForList> Comments { get; set; }

        public int CurrentParentId { get; set; }

        public string CurrentParentName { get; set; }

        public int CurrentQuoteId { get; set; }

        public string CurrentQuoteText { get; set; }

        public string GameKey { get; set; }

        public string NewName { get; set; }

        public string NewBody { get; set; }

        public bool IsDeletedGame { get; set; }

        public int UserId { get; set; }

        public UserViewModel CurrentUser { get; set; }
    }
}