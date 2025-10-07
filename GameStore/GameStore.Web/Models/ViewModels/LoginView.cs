using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.ViewModels
{
    public class LoginView
    {
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EnterEmail")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "EnterPassword")]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}