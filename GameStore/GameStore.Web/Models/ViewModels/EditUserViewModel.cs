using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), 
            ErrorMessageResourceName = "EnterName")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EnterSurname")]
        public string Surname { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EnterEmail")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EnterPassword")]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }

        public bool IsBanned { get; set; }

        public int PublisherId { get; set; }

        public PublisherViewModel Publisher { get; set; }

        public DateTime BannedUntil { get; set; }

        public SelectList Publishers { get; set; }

        public List<RoleViewModel> Roles { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "ChooseAnyRole")]
        public List<int> RoleIds { get; set; }

        public List<RoleViewModel> AllRoles { get; set; }

        public bool IsDeleted { get; set; }

        public int SenderTypeId { get; set; }
    }
}