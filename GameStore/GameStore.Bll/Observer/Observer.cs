using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.Bll.Observer.Interfaces;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Observer
{
    public class Observer : IObserver
    {
        private readonly IUnitOfWork _unitOfWork;

        public List<Manager> Managers { get; set; }

        public Observer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Managers = new List<Manager>();
        }

        public Observer GetObservers()
        {
            var managers = _unitOfWork.UserRepository
                .Get()
                .Where(user => user.Roles.Any(role => role.Name == "Manager")).ToList();
            Managers = Mapper.Map<List<Manager>>(managers);

            return this;
        }
    }
}