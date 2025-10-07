namespace GameStore.Dal.Entities
{
    public class Comment : BaseEntity
    {
        public Comment()
        {
        }

        public string Name { get; set; }

        public string Body { get; set; }

        public int GameId { get; set; }

        public int UserId { get; set; }

        public int ParentId { get; set; }

        public int QuoteId { get; set; }
    }
}
