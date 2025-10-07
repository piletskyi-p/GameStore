using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LanguageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<LanguageDto> GetAll()
        {
            var languages = _unitOfWork.LanguageRepository.Get();
            var languagesDto = Mapper.Map<IEnumerable<LanguageDto>>(languages);

            return languagesDto;
        }

        public LanguageDto GetById(int id)
        {
            var language = _unitOfWork.LanguageRepository.FindById(id);
            var langDto = Mapper.Map<LanguageDto>(language);

            return langDto;
        }

        public LanguageDto GetByKey(string key)
        {
            var language = _unitOfWork.LanguageRepository.Get(lang => lang.Key == key).FirstOrDefault();
            var langDto = Mapper.Map<LanguageDto>(language);

            return langDto;
        }
    }
}