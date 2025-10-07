namespace GameStore.Bll.DTO.TranslateDto
{
    public class PublisherTranslateDto
    {
        public int Id { get; set; }

        public int PublisherId { get; set; }

        public string Description { get; set; }

        public int LanguageId { get; set; }

        public LanguageDto Language { get; set; }
    }
}