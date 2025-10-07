using System;
using System.Web.Mvc;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;
using LoginView = GameStore.Web.Models.ViewModels.LoginView;

namespace GameStore.Web.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IBanService _banService;

        public LoginController(IBanService banService, IAuthentication auth, ILanguageService languageService) : base(auth)
        {
            _banService = banService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View("Index", new LoginView());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                var user = Auth.Login(loginView.Email, loginView.Password, loginView.IsPersistent);
                if (user != null)
                {
                    if (user.BannedUntil < DateTime.UtcNow)
                    {
                        _banService.UnBan(user.Id);
                    }
                    
                    return RedirectToAction("UserPage", "User");
                }

                ModelState["Password"].Errors.Add("This account doesn't exist.");
            }

            return View("Index", loginView);
        }

        //[AllowAnonymous]
        //[Authorize(Roles = "Publisher, Manager, Moderator, User, Administrator, anonym")]
        public ActionResult Logout()
        {
            Auth.LogOut();
            return RedirectToAction("Filter", "Game");
        }
    }
}