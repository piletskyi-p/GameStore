using System;
using System.Globalization;
using System.Net;
using System.Web.Helpers;
using System.Web.Http;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers.Api
{
    public class LoginsController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IUserTokenService _userTokenService;

        public LoginsController(IUserService userService, IUserTokenService userTokenService)
        {
            _userService = userService;
            _userTokenService = userTokenService;
        }

      
        [HttpPost]
        public IHttpActionResult Login([FromBody]LoginUser loginUser)
        {
            var user = _userService.Login(loginUser.Email, loginUser.Password);
            if (user == null)
            {
                return Content(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            var token = new UserTokenDto
            {
                UserId = user.Id,
                Token = Crypto.HashPassword(user.Email + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture))
            };

            _userTokenService.Create(token);
            var currentToken = _userTokenService.GetByUserId(user.Id);
            return Content(HttpStatusCode.OK, currentToken);
        }
    }
}