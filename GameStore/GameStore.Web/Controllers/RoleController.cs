using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;
using GameStore.Web.Models.ViewModels;

namespace GameStore.Web.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(
            IRoleService role, IAuthentication auth) : base(auth)
        {
            _roleService = role;
        }

        [HttpGet]
        public ActionResult AllRoles()
        {
            var rolesDto = _roleService.GetAll();
            var roles = Mapper.Map<List<RoleDTO>>(rolesDto);

            return View("AllRoles", roles);
        }

        [HttpGet]
        public ActionResult NewRole()
        {
            return View("NewRole", new RoleViewModel());
        }

        [HttpPost]
        public ActionResult NewRole(RoleViewModel newRole)
        {
            if (ModelState.IsValid)
            {
                var newRoleDto = Mapper.Map<RoleDTO>(newRole);
                _roleService.AddRole(newRoleDto);
                return AllRoles();
            }

            return View("NewRole", newRole);
        }
    }
}