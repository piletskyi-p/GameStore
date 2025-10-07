using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Models.LanguageModels;

namespace GameStore.Web.Models.ViewModels
{
    public class GameViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EnterKey")]
        public string Key { get; set; }

        public string Name { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EnterBiggerThenNull")]
        [Required]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EnterBiggerThenNull")]
        [Required]
        public int UnitInStock { get; set; }

        public bool IsDiscontinued { get; set; }

        public bool IsDeleted { get; set; }

        public double Rating { get; set; }

        public List<CommentForList> Comments { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ChooseAnyGenre")]
        public List<int> GenresIds { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ChooseAnyPlatform")]
        public List<int> PlatformsIds { get; set; }

        public SelectList PublisherSelect { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public DateTime UploadDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ChooseAnyDate")]
        [ScaffoldColumn(false)]
        public DateTime PublicationDate { get; set; }

        [ScaffoldColumn(false)]
        public int PublisherId { get; set; }

        public PublisherViewModel Publisher { get; set; }

        public List<GenreViewModel> Genres { get; set; }

        public List<PlatformViewModel> Platforms { get; set; }

        public List<GameTranslateModel> GameTranslates { get; set; }

        public int ImageId { get; set; }

        public ImageViewModel Image { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}