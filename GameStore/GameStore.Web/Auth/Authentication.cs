using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;

namespace GameStore.Web.Auth
{
    public class Authentication : IAuthentication
    {
        private const string cookieName = "__AUTH_COOKIE";

        private readonly IUserService _userService;

        private IPrincipal _currentUser;

        public Authentication(IUserService userService)
        {
            _userService = userService;
        }

        public HttpContext HttpContext { get; set; }

        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    try
                    {
                        HttpCookie authCookie = HttpContext.Request.Cookies.Get(cookieName);
                        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            _currentUser = new UserProvider(ticket.Name, _userService);
                        }
                        else
                        {
                            _currentUser = new UserProvider(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        _currentUser = new UserProvider(null, null);
                    }
                }

                return _currentUser;
            }
        }

        public UserDTO Login(string userName, string password, bool isPersistent)
        {

            var retUser = _userService.Login(userName, password);
            if (retUser != null)
            {
                    CreateCookie(userName, isPersistent);
            }

            return retUser;
        }

        public void LogOut()
        {
            var httpCookie = HttpContext.Response.Cookies[cookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        private void CreateCookie(string userName, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                userName,
                DateTime.UtcNow,
                DateTime.UtcNow.Add(FormsAuthentication.Timeout),
                isPersistent,
                string.Empty,
                FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            var authCookie = new HttpCookie(cookieName)
            {
                Value = encTicket,
                Expires = DateTime.UtcNow.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(authCookie);
        }
    }
}