using System;
using System.Linq;
using GameStore.Bll.Interfaces;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Services
{
    public class BanService : IBanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventLogger _eventService;

        public BanService(IUnitOfWork unitOfWork, IEventLogger baseService)
        {
            _eventService = baseService;
            _unitOfWork = unitOfWork;
        }

        public void Ban(int commentId, string period)
        {
            var com = _unitOfWork.CommentRepository
                .GetFromAll(comment => comment.Id == commentId).FirstOrDefault();
            int userId = 0;

            if (com != null)
            {
                userId = com.UserId;
            }

            var user = _unitOfWork.UserRepository.GetFromAll(us => us.Id == userId).FirstOrDefault();
            if (user != null)
            {
                user.IsBanned = true;
                switch (period)
                {
                    case "hour":
                        user.BannedUntil = DateTime.UtcNow.AddHours(1);
                        break;
                    case "day":
                        user.BannedUntil = DateTime.UtcNow.AddDays(1);
                        break;
                    case "week":
                        user.BannedUntil = DateTime.UtcNow.AddDays(7);
                        break;
                    case "month":
                        user.BannedUntil = DateTime.UtcNow.AddMonths(1);
                        break;
                    case "permanent":
                        user.BannedUntil = DateTime.UtcNow.AddYears(100);
                        break;
                }

                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
                var userOld = user;
                userOld.IsBanned = false;
                _eventService.LogUpdate(userOld, user);
            }
        }

        public void UnBan(int userId)
        {
            var user = _unitOfWork.UserRepository
                .GetFromAll(us => us.Id == userId).FirstOrDefault();
            if (user != null)
            {
                user.IsBanned = false;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
            }
        }
    }
}