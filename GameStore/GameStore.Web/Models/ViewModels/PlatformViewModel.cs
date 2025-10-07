using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.Models.LanguageModels;

namespace GameStore.Web.Models.ViewModels
{
    public class PlatformViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Type { get; set; }

        public List<int> PlatformsIds { get; set; }

        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }

        public List<GameViewModel> Games { get; set; }

        public List<PlatformTranslateModel> PlatformTranslates { get; set; }
    }
}