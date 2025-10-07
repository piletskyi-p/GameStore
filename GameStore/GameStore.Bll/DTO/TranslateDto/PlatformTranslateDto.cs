namespace GameStore.Bll.DTO.TranslateDto
{
    public class PlatformTranslateDto
    {
        public int Id { get; set; }

        public int PlatformId { get; set; }

        public string Type { get; set; }

        public int LanguageId { get; set; }

        public LanguageDto Language { get; set; }
    }
}