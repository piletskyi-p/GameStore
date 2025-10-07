using System.Collections.Generic;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Services
{
    public class ShippersService : IShippersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShippersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ShippersDTO> Get()
        {
            var shippers = _unitOfWork.MongoShippersRepository.Get();
            var shippersDto = Mapper.Map<IEnumerable<ShippersDTO>>(shippers);

            return shippersDto;
        }
    }
}