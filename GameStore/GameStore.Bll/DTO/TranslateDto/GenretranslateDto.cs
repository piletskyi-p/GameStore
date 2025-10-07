namespace GameStore.Bll.DTO.TranslateDto
{
    public class GenreTranslateDto
    {
        public int Id { get; set; }

        public int GenreId { get; set; }

        public string Name { get; set; }

        public int LanguageId { get; set; }

        public LanguageDto Language { get; set; }
    }
}