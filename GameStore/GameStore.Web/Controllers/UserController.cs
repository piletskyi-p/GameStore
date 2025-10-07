using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;
using GameStore.Web.Models.ViewModels;
using NLog;

namespace GameStore.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IPublisherService _publisherService;
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;

        public UserController(
            IUserService userService,
            ILogger logger,
            IRoleService roleService,
            IPublisherService publisherService,
            IAuthentication auth,
            ILanguageService languageService) : base(auth)
        {
            _userService = userService;
            _logger = logger;
            _roleService = roleService;
            _publisherService = publisherService;
            _languageService = languageService;
        }

        [Authorize(Roles = "User, Administrator, Manager, Moderator")]
        public ActionResult UserPage()
        {
            _logger.Info("UserPage");
            var userEmail = Auth.CurrentUser.Identity.Name;
            var userDto = _userService.GetUser(userEmail);
            var user = Mapper.Map<UserViewModel>(userDto);

            return View("UserPage", user);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View("Register", new UserViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(UserViewModel newUser)
        {
            var user = _userService.GetUser(newUser.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "User with this email has already exist.");
            }

            if (ModelState.IsValid)
            {
                var userDto = Mapper.Map<UserDTO>(newUser);
                //userDto.Password = Crypto.HashPassword(userDto.Password);
                userDto.Password = userDto.Password;
                _userService.Register(userDto);

                return RedirectToAction("Index", "Login");
            }

            return View("Register", newUser);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult GetAllUsers()
        {
            var usersDto = _userService.GetAllUsers();
            var users = Mapper.Map<IEnumerable<UserViewModel>>(usersDto);
            if (users.Any())
            {
                return View("AllUsers", users);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditUser(int id, string lang = "en")
        {
            var userDto = _userService.GetUserById(id);
            if (userDto != null)
            {
                var user = Mapper.Map<EditUserViewModel>(userDto);
                var rolesDto = _roleService.GetAll();
                var publishersDto = Mapper
                    .Map<IEnumerable<PublisherViewModel>>(_publisherService.GetAllPublisher(lang));
                user.AllRoles = Mapper.Map<List<RoleViewModel>>(rolesDto);
                user.Publishers = new SelectList(publishersDto, "Id", "CompanyName");
                return View("EditUser", user);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditUser(EditUserViewModel user, string lang = "en")
        {
            if (ModelState.IsValid)
            {
                var userDto = Mapper.Map<UserDTO>(user);
                _userService.Edit(userDto);

                return RedirectToAction("GetAllUsers");
            }

            var rolesDto = _roleService.GetAll();
            var publishersDto = Mapper
                .Map<IEnumerable<PublisherViewModel>>(_publisherService.GetAllPublisher(lang));
            user.AllRoles = Mapper.Map<List<RoleViewModel>>(rolesDto);
            user.Roles = new List<RoleViewModel>();
            user.Publishers = new SelectList(publishersDto, "Id", "CompanyName");
            return View("EditUser", user);
        }

        [HttpGet]
        public ActionResult EditSender()
        {
            var userEmail = Auth.CurrentUser.Identity.Name;
            var userDto = _userService.GetUser(userEmail);
            var user = Mapper.Map<UserViewModel>(userDto);

            return View("EditSender", user);
        }


        [HttpPost]
        public ActionResult EditSender(int senderTypeId)
        {
            var userEmail = Auth.CurrentUser.Identity.Name;
            _userService.EditSender(userEmail, senderTypeId);

            return UserPage();
        }
    }
}