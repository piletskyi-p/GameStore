namespace GameStore.Bll.DTO.TranslateDto
{
    public class GameTranslateDto
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public string Description { get; set; }

        public int LanguageId { get; set; }

        public LanguageDto Language { get; set; }
    }
}