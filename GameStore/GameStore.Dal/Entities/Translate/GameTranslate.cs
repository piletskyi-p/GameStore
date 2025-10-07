namespace GameStore.Dal.Entities.Translate
{
    public class GameTranslate : BaseEntity
    {
        public int? GameId { get; set; }

        public string Description { get; set; }

        public int? LanguageId { get; set; }

        public virtual Language Language { get; set; }
    }
}