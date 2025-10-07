namespace GameStore.Dal.Entities.Translate
{
    public class GenreTranslate : BaseEntity
    {
        public int? GenreId { get; set; }

        public string Name { get; set; }

        public int? LanguageId { get; set; }

        public virtual Language Language { get; set; }
    }
}