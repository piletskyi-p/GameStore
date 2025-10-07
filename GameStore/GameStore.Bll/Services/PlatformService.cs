using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Translate;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventLogger _eventLogger;

        public PlatformService(IUnitOfWork unitOfWork, IEventLogger eventLogger)
        {
            _unitOfWork = unitOfWork;
            _eventLogger = eventLogger;
        }

        public List<PlatformDTO> GetAll(string lang)
        {
            var platforms = _unitOfWork.PlatformRepository.Get(
                repository => repository.Games,
                repository => repository.PlatformTranslates,
                repository => repository.PlatformTranslates.Select(platf => platf.Language)).ToList();

            if (platforms.Count != 0)
            {
                var platformsDto = Mapper.Map<List<Platform>, List<PlatformDTO>>(platforms);
                for (int i = 0; i < platforms.Count(); ++i)
                {
                    platformsDto[i].Type = platforms[i].PlatformTranslates
                        .FirstOrDefault(tr => tr.Language.Key == lang)
                        ?.Type;
                }

                return platformsDto;
            }

            return new List<PlatformDTO>();
        }

        public IEnumerable<PlatformDTO> GetByGameKey(string key, string lang)
        {
            var game = _unitOfWork.GameRepository.Get(gm => gm.Key == key,
            repository => repository.Platforms,
                repository => repository.Platforms.Select(platf => platf.PlatformTranslates),
                repository => repository.Platforms.Select(platf => platf.Games)).FirstOrDefault();

            if (game != null)
            {
                var platforms = game.Platforms.ToList();
                var platformDto = Mapper.Map<List<Platform>, List<PlatformDTO>>(platforms);
                for (int i = 0; i < platforms.Count(); ++i)
                {
                    platformDto[i].Type = platforms[i].PlatformTranslates
                        .FirstOrDefault(tr => tr.Language.Key == lang)
                        ?.Type;
                }

                return platformDto;
            }

            return new List<PlatformDTO>();
        }

        public void Create(PlatformDTO platformDto)
        {
            var platform = Mapper.Map<Platform>(platformDto);
            foreach (var translate in platform.PlatformTranslates)
            {
                translate.Language = _unitOfWork.LanguageRepository.FindById(translate.LanguageId);
            }

            _unitOfWork.PlatformRepository.Create(platform);
            _unitOfWork.Save();
            _eventLogger.LogCreate(Mapper.Map<Platform>(platform));
        }

        public void Delete(int id)
        {
            var platform = _unitOfWork.PlatformRepository.FindById(id);
            _unitOfWork.PlatformRepository.Delete(id);
            _unitOfWork.Save();
            _eventLogger.LogDelete(Mapper.Map<Platform>(platform));
        }

        public PlatformDTO GetById(int id, string lang)
        {
            var platform = _unitOfWork.PlatformRepository.Get(pl => pl.Id == id,
                repository => repository.Games,
                repository => repository.PlatformTranslates,
                repository => repository.PlatformTranslates.Select(platf => platf.Language))
                .FirstOrDefault();

            if (platform != null)
            {
                var platformDto = Mapper.Map<PlatformDTO>(platform);
                platformDto.Type = platform.PlatformTranslates
                    .FirstOrDefault(tr => tr.Language.Key == lang)?.Type;

                return platformDto;
            }

            return null;
        }

        public void Update(PlatformDTO platformDto)
        {
            var platform = _unitOfWork.PlatformRepository.FindById(platformDto.Id,
                repository => repository.PlatformTranslates,
                repository => repository.PlatformTranslates.Select(platf => platf.Language),
                repository => repository.Games);

            var type = platform.PlatformTranslates;

            platform.PlatformTranslates = Mapper.Map<ICollection<PlatformTranslate>>(platformDto.PlatformTranslates);
            _unitOfWork.PlatformRepository.Update(platform);
            _unitOfWork.Save();

            platform.PlatformTranslates = type;
            platformDto.Games = null;

            _eventLogger.LogUpdate(Mapper.Map<Platform>(platform), Mapper.Map<Platform>(platformDto));
        }
    }
}