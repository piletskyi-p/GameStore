using System.Security.Principal;
using GameStore.Bll.Auth;
using GameStore.Bll.Interfaces;

namespace GameStore.Web.Auth
{
    public class UserProvider : IPrincipal
    {
        public UserProvider(string name, IUserService service)
        {
            UserIdentity = new UserIndentity(name, service);
        }

        public IIdentity Identity => UserIdentity;

        private UserIndentity UserIdentity { get; set; }

        public bool IsInRole(string role)
        {
            if (UserIdentity.User == null)
            {
                return false;
            }

            return UserIdentity.User.InRoles(role);
        }

        public override string ToString()
        {
            return UserIdentity.Name;
        }
    }
}
