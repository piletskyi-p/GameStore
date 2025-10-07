using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;

namespace GameStore.Web.Auth.Api
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private readonly IUserService _userService =
            DependencyResolver.Current.GetService<IUserService>();
        private readonly IUserTokenService _userTokenService =
            DependencyResolver.Current.GetService<IUserTokenService>();

        public string UserToken { get; set; }

        public UserDTO CheckCredential(string token)
        {
            var id = _userTokenService.GetUserIdByToken(token);
            if (id == 0)
            {
                return null;
            }

            var user = _userService.GetUserById(id);

            return user;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var token = request.Headers.GetValues("Authorization").FirstOrDefault();
                if (token != null)
                {
                    var objUser = CheckCredential(token);
                    if (objUser != null)
                    {
                        var roles = objUser.Roles.Select(role => role.Name).ToArray();
                        IPrincipal principal = new GenericPrincipal(new GenericIdentity(objUser.Email), roles);
                        Thread.CurrentPrincipal = principal;
                        HttpContext.Current.User = principal;
                        UserToken = token;
                    }
                }

                return base.SendAsync(request, cancellationToken);
            }
            catch
            {
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}