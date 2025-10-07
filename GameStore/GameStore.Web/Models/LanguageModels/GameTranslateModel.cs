using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.LanguageModels
{
    public class GameTranslateModel
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EnterDescription")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int LanguageId { get; set; }

        public LanguageModel Language { get; set; }
    }
}