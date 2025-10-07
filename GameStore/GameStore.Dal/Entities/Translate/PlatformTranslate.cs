namespace GameStore.Dal.Entities.Translate
{
    public class PlatformTranslate : BaseEntity
    {
        public int? PlatformId { get; set; }

        public string Type { get; set; }

        public int? LanguageId { get; set; }

        public virtual Language Language { get; set; }
    }
}