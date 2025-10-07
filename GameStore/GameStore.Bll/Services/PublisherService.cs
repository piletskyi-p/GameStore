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
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventLogger _eventLogger;

        public PublisherService(IUnitOfWork unitOfWork, IEventLogger baseService)
        {
            _unitOfWork = unitOfWork;
            _eventLogger = baseService;
        }

        public IEnumerable<PublisherDTO> GetAllPublisher(string lang)
        {
            var publisher = _unitOfWork.PublisherRepository.Get().ToList();

            var publisherDto = Mapper.Map<List<Publisher>, List<PublisherDTO>>(publisher);

            return publisherDto;
        }

        public void NewPublisher(PublisherDTO publisherDto)
        {
            publisherDto.Games = new List<GameDTO>();
            var publisher = Mapper.Map<PublisherDTO, Publisher>(publisherDto);
            foreach (var translate in publisher.PublisherTranslate)
            {
                translate.Language = _unitOfWork.LanguageRepository.FindById(translate.LanguageId);
            }

            _unitOfWork.PublisherRepository.Create(publisher);
            _unitOfWork.Save();
            _eventLogger.LogCreate(Mapper.Map<Publisher>(publisher));
        }

        public PublisherDTO GetById(int id, string lang)
        {
            var publisher = _unitOfWork.PublisherRepository.Get(Id => Id.Id == id,
                repository => repository.PublisherTranslate,
                repository => repository.PublisherTranslate.Select(translate => translate.Language),
                repository => repository.Users,
                repository => repository.Users.Select(users => users.Roles),
                repository => repository.Games).FirstOrDefault();

            if (publisher == null)
            {
                return null;
            }

            var pub = Mapper.Map<Publisher, PublisherDTO>(publisher);
            pub.Description = publisher.PublisherTranslate
                .FirstOrDefault(tr => tr.Language.Key == lang)?.Description;

            return pub;
        }

        public PublisherDTO GetByName(string name, string lang)
        {
            var publisher = _unitOfWork.PublisherRepository.Get(id => id.CompanyName == name,
                repository => repository.PublisherTranslate,
                repository => repository.PublisherTranslate.Select(translate => translate.Language),
                repository => repository.Users,
                repository => repository.Users.Select(users => users.Roles),
                repository => repository.Games).FirstOrDefault();

            if (publisher == null)
            {
                return null;
            }

            var pub = Mapper.Map<Publisher, PublisherDTO>(publisher);
            pub.Description = publisher.PublisherTranslate
                .FirstOrDefault(tr => tr.Language.Key == lang)?.Description;

            return pub;
        }

        public void RemovePublisher(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var publisherTemp = _unitOfWork.PublisherRepository.Get(pub => pub.CompanyName == name);
                var publisher = publisherTemp.FirstOrDefault();

                if (publisher != null)
                {
                    publisher.IsDeleted = true;
                    _unitOfWork.PublisherRepository.Update(publisher);
                    _unitOfWork.Save();
                    _eventLogger.LogDelete(Mapper.Map<Publisher>(publisher));
                }
            }
        }

        public void RemovePublisherById(int id)
        {
            if (id != 0)
            {
                var publisherTemp = _unitOfWork.PublisherRepository.Get(pub => pub.Id == id);
                var publisher = publisherTemp.FirstOrDefault();

                if (publisher != null)
                {
                    publisher.IsDeleted = true;
                    _unitOfWork.PublisherRepository.Update(publisher);
                    _unitOfWork.Save();
                    _eventLogger.LogDelete(Mapper.Map<Publisher>(publisher));
                }
            }
        }

        public void EditPublisher(PublisherDTO publisherDto)
        {
            var publisher = _unitOfWork.PublisherRepository.FindById(publisherDto.Id,
                repository => repository.PublisherTranslate,
                repository => repository.PublisherTranslate.Select(translate => translate.Language));
            string companyNameStr = publisher.CompanyName;
            var descriptionStr = publisher.PublisherTranslate;
            string homePageStr = publisher.HomePage;

            publisher.CompanyName = publisherDto.CompanyName;
            publisher.PublisherTranslate = Mapper.Map<ICollection<PublisherTranslate>>(publisherDto.PublisherTranslate);
            publisher.HomePage = publisherDto.HomePage;
            _unitOfWork.PublisherRepository.Update(publisher);
            _unitOfWork.Save();

            publisher.CompanyName = companyNameStr;
            publisher.PublisherTranslate = descriptionStr;
            publisher.HomePage = homePageStr;
            publisherDto.Games = null;

            _eventLogger.LogUpdate(Mapper.Map<Publisher>(publisher), Mapper.Map<Publisher>(publisherDto));
        }
    }
}