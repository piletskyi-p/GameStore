using System.Web;
using GameStore.Bll.DTO;

namespace GameStore.Web.Auth
{
    public interface IAuthentication
    {
        HttpContext HttpContext { get; set; }

        System.Security.Principal.IPrincipal CurrentUser { get; }

        UserDTO Login(string login, string password, bool isPersistent);

        void LogOut();
    }
}