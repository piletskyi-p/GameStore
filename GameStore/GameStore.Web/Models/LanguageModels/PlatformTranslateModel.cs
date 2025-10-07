using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.LanguageModels
{
    public class PlatformTranslateModel
    {
        public int Id { get; set; }

        public int PlatformId { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EnterName")]
        public string Type { get; set; }

        public int? LanguageId { get; set; }

        public LanguageModel Language { get; set; }
    }
}