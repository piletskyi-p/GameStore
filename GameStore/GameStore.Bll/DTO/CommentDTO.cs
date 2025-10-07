namespace GameStore.Bll.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public int ParentId { get; set; }

        public bool IsDeleted { get; set; }

        public int GameId { get; set; }

        public int UserId { get; set; }

        public UserDTO User { get; set; }

        public GameDTO Game { get; set; }

        public int QuoteId { get; set; }
    }
}