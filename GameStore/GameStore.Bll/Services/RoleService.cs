using System.Collections.Generic;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventLogger _eventService;

        public RoleService(IUnitOfWork unitOfWork, IEventLogger baseService)
        {
            _unitOfWork = unitOfWork;
            _eventService = baseService;
        }

        public void AddRole(RoleDTO roleDto)
        {
            var role = Mapper.Map<Role>(roleDto);
            _unitOfWork.RoleRepository.Create(role);
            _unitOfWork.Save();
            _eventService.LogCreate(role);
        }

        public IEnumerable<RoleDTO> GetAll()
        {
            var roles = _unitOfWork.RoleRepository.Get(repository => repository.Users);
            var rolesDto = Mapper.Map<IEnumerable<RoleDTO>>(roles);

            return rolesDto;
        }
    }
}