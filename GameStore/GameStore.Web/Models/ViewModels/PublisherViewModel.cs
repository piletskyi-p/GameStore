using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.Models.LanguageModels;

namespace GameStore.Web.Models.ViewModels
{
    public class PublisherViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public string HomePage { get; set; }

        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }

        public List<GameViewModel> Games { get; set; }

        public List<PublisherTranslateModel> PublisherTranslate { get; set; }
    }
}