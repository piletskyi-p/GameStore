using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using NLog;

namespace GameStore.Bll.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public UserService(ILogger logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void Edit(UserDTO userDto)
        {
            var user = _unitOfWork.UserRepository.Get(us => us.Id == userDto.Id).FirstOrDefault();
            Mapper.Map(userDto, user);

            if (user != null)
            {
                user.Roles = userDto.RoleIds.Select(id => _unitOfWork.RoleRepository
                    .FindById(id)).ToList();
                if (userDto.PublisherId != 0 && user.Roles.Any(role => role.Name == "Publisher"))
                {
                    user.Publisher = _unitOfWork.PublisherRepository.FindById(userDto.PublisherId);
                }

                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
            }
        }

        public void EditSender(string email, int id)
        {
            var user = _unitOfWork.UserRepository.Get(us => us.Email == email).FirstOrDefault();
            if (user != null)
            {
                user.SenderTypeId = id;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = _unitOfWork.UserRepository.Get(repository => repository.Publisher,
                repository => repository.Roles);
            var usersDto = Mapper.Map<IEnumerable<UserDTO>>(users);

            return usersDto;
        }

        public UserDTO GetUser(string email)
        {
            var user = _unitOfWork.UserRepository.Get(users => users.Email == email,
                    repository => repository.Publisher,
                    repository => repository.Roles,
                    repository => repository.Rates,
                    repository => repository.Rates.Select(i => i.Game))
                .FirstOrDefault();
            var userDto = Mapper.Map<UserDTO>(user);

            return userDto;
        }

        public UserDTO GetUserById(int id)
        {
            var user = _unitOfWork.UserRepository.Get(us => us.Id == id,
                    repository => repository.Publisher,
                    repository => repository.Roles)
                .FirstOrDefault();
            var userDto = Mapper.Map<UserDTO>(user);

            return userDto;
        }

        public UserDTO Login(string email, string password)
        {
            var user = _unitOfWork.UserRepository
                .Get(users => users.Email == email,
                    repository => repository.Publisher,
                    repository => repository.Roles)
                .FirstOrDefault();

            // if (user != null && Crypto.VerifyHashedPassword(user.Password, password))
            if (user != null && user.Password == password)
            {
                var userDto = Mapper.Map<UserDTO>(user);
                _logger.Info("User sing in");
                return userDto;
            }

            return null;
        }

        public void Register(UserDTO userDto)
        {
            var user = Mapper.Map<User>(userDto);
            var role = _unitOfWork.RoleRepository
                .Get(roles => roles.Name == "User").FirstOrDefault();
            user.Roles = new List<Role> { role };
            user.BannedUntil = DateTime.UtcNow;
            //user.Password = Crypto.HashPassword(user.Password);
            user.Password = user.Password;

            _unitOfWork.UserRepository.Create(user);
            _unitOfWork.Save();
        }
    }
}