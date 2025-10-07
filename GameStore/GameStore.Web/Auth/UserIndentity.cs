using System.Security.Principal;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;

namespace GameStore.Bll.Auth
{
    public class UserIndentity : IUserProvider, IIdentity
    {

        public UserIndentity(string email, IUserService service)
        {
            if (!string.IsNullOrEmpty(email))
            {
                User = service.GetUser(email);
            }
        }

        public IAuthentication Auth { get; set; }

        public UserDTO CurrentUser
        {
            get
            {
                return ((IUserProvider)Auth.CurrentUser.Identity).User;
            }
        }

       public UserDTO User { get; set; }

        public string AuthenticationType
        {
            get
            {
                return typeof(UserDTO).ToString();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        public string Name
        {
            get
            {
                if (User != null)
                {
                    return User.Email;
                }

                return "anonym";
            }
        }

        public string NameSurname
        {
            get
            {
                if (User != null)
                {
                    return User.Name + " " + User.Surname;
                }

                return "anonym";
            }
        }
    }
}