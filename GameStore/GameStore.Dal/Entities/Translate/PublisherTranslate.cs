namespace GameStore.Dal.Entities.Translate
{
    public class PublisherTranslate : BaseEntity
    {
        public int? PublisherId { get; set; }

        public string Description { get; set; }
        
        public int? LanguageId { get; set; }

        public virtual Language Language { get; set; }
    }
}
