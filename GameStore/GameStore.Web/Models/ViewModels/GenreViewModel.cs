using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.Models.LanguageModels;

namespace GameStore.Web.Models.ViewModels
{
    public class GenreViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Name { get; set; }

        [ScaffoldColumn(false)]
        public int? ParentId { get; set; }

        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }

        [ScaffoldColumn(false)]
        public string ParentName { get; set; }

        public GenreViewModel Parent { get; set; }

        public List<GameViewModel> Games { get; set; }

        public List<GenreTranslateModel> GenreTranslates { get; set; }
    }
}
