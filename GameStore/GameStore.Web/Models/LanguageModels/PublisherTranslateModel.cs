using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.LanguageModels
{
    public class PublisherTranslateModel
    {
        public int Id { get; set; }

        public int PublisherId { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EnterName")]
        public string Description { get; set; }

        public int? LanguageId { get; set; }

        public LanguageModel Language { get; set; }
    }
}