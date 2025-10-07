using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels
{
    public class CommentForList
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public int ParentId { get; set; }

        [Required]
        public string ParentName { get; set; }

        [Required]
        public string ParentComment { get; set; }

        public int UserId { get; set; }

        [Required]
        public int GameId { get; set; }

        public string GameKey { get; set; }

        public int QuoteId { get; set; }

        public string QuoteText { get; set; }

        public GameViewModel Game { get; set; }

        public UserViewModel User { get; set; }
    }
}