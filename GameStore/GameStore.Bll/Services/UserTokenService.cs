using System;
using System.Linq;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using NLog;

namespace GameStore.Bll.Services
{
    public class UserTokenService : IUserTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public UserTokenService(ILogger logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void Create(UserTokenDto userToken)
        {
            var token = Mapper.Map<UserToken>(userToken);
            token.CreateDateTime = DateTime.UtcNow;
            token.DropDateTime = DateTime.UtcNow.AddMinutes(30);

            _unitOfWork.UserTokenRepository.Create(token);
            _unitOfWork.Save();
        }

        public UserTokenDto GetById(int id)
        {
            var token = _unitOfWork.UserTokenRepository.FindById(id);
            var tokenDto = Mapper.Map<UserTokenDto>(token);

            return tokenDto;
        }

        public UserTokenDto GetByToken(string token)
        {
            var userToken = _unitOfWork.UserTokenRepository
                .Get(tokens => tokens.Token == token).FirstOrDefault();
            if (userToken != null)
            {
                if (userToken.DropDateTime > DateTime.UtcNow)
                {
                    var tokenDto = Mapper.Map<UserTokenDto>(userToken);

                    return tokenDto;
                }

                _unitOfWork.UserTokenRepository.Delete(userToken.Id);
                _unitOfWork.Save();
            }

            return null;
        }

        public UserTokenDto GetByUserId(int userId)
        {
            var token = _unitOfWork.UserTokenRepository
                .Get(tok => tok.UserId == userId).FirstOrDefault();
            var tokenDto = Mapper.Map<UserTokenDto>(token);

            return tokenDto;
        }

        public int GetUserIdByToken(string token)
        {
            var userToken = _unitOfWork.UserTokenRepository
                .Get(tokens => tokens.Token == token).FirstOrDefault();
            if (userToken != null)
            {
                if (userToken.DropDateTime > DateTime.UtcNow)
                {
                    return userToken.UserId;
                }

                _unitOfWork.UserTokenRepository.Delete(userToken.Id);
                _unitOfWork.Save();
            }

            return 0;
        }
    }
}