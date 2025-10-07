using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.DTO.TranslateDto;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Interfaces;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Translate;
using GameStore.Dal.Interfaces;
using GameStore.Web;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServiceTests
{
    public class PublisherServiceTests
    {
        private readonly Mock<IEventLogger> _baseService;
        private readonly Mock<IMapper> _mapper;
        private PublisherService _publisherService;
        private Mock<IUnitOfWork> _unitOfWork;

        public PublisherServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _baseService = new Mock<IEventLogger>();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _publisherService = new PublisherService(_unitOfWork.Object, _baseService.Object);
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutomapperWebProfile>();
                cfg.AddProfile<MapProfile>();
            });
        }

        [Test]
        public void NewPublisher_New_SaveOnce()
        {
            _mapper.Setup(mapper => mapper.Map<PublisherDTO, Publisher>(
                It.IsAny<PublisherDTO>())).Returns(It.IsAny<Publisher>());
            _unitOfWork.Setup(p => p.PublisherRepository.Create(It.IsAny<Publisher>()));
            _baseService
                .Setup(service => service.LogCreate(It.IsAny<Publisher>()));

            _publisherService.NewPublisher(new PublisherDTO());
            _unitOfWork.Verify(create => create.Save(), Times.Once);
        }

        [Test]
        public void GetAllPublisher_Get_GetList()
        {
            IEnumerable<PublisherDTO> publisherDto = new List<PublisherDTO>
            {
                new PublisherDTO()
            };
            IEnumerable<Publisher> publisher = new List<Publisher>
            {
                new Publisher()
            };
            _mapper.Setup(mapper => mapper.Map<IEnumerable<Publisher>, IEnumerable<PublisherDTO>>(It.IsAny<IEnumerable<Publisher>>())).Returns(publisherDto);
            _unitOfWork.Setup(p => p.PublisherRepository.Get()).Returns(publisher);

            var result = _publisherService.GetAllPublisher("lang");
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetById_Get_GetNull()
        {
            var pub = new List<Publisher>
            {
                new Publisher
                {
                    PublisherTranslate = new List<PublisherTranslate>()
                }
            };
            _unitOfWork.Setup(p => p.PublisherRepository
                .Get(It.IsAny<Expression<Func<Publisher, bool>>>(),
                    It.IsAny<Expression<Func<Publisher, object>>[]>())).Returns(pub);

            var result = _publisherService.GetById(1, "lang");
            Assert.AreEqual(typeof(PublisherDTO), result.GetType());
        }

        [Test]
        public void GetById_Get_GetNotNull()
        {
            var pub = new List<Publisher>
            {
                new Publisher
                {
                    PublisherTranslate = new List<PublisherTranslate>()
                }
            };
            var pubDto = new PublisherDTO();
            _unitOfWork.Setup(p => p.PublisherRepository
                .Get(It.IsAny<Expression<Func<Publisher, bool>>>(),
                    It.IsAny<Expression<Func<Publisher, object>>[]>())).Returns(pub);
            _mapper.Setup(mapper => mapper.Map<Publisher, PublisherDTO>(It.IsAny<Publisher>())).Returns(pubDto);

            var result = _publisherService.GetById(1, "lang");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetByName_Get_GetNull()
        {
            var pub = new List<Publisher>
            {
                new Publisher
                {
                    PublisherTranslate = new List<PublisherTranslate>()
                }
            };
            _unitOfWork.Setup(p => p.PublisherRepository
                .Get(It.IsAny<Expression<Func<Publisher, bool>>>(),
                    It.IsAny<Expression<Func<Publisher, object>>[]>())).Returns(pub);

            var result = _publisherService.GetByName(string.Empty, "lang");
            Assert.AreEqual(typeof(PublisherDTO), result.GetType());
        }

        [Test]
        public void GetByName_Get_GetNotNull()
        {
            var pub = new List<Publisher>
            {
                new Publisher
                {
                    PublisherTranslate = new List<PublisherTranslate>()
                }
            };
            var pubDto = new PublisherDTO();
            _unitOfWork.Setup(p => p.PublisherRepository
                .Get(It.IsAny<Expression<Func<Publisher, bool>>>(),
                    It.IsAny<Expression<Func<Publisher, object>>[]>())).Returns(pub);
            _mapper.Setup(mapper => mapper.Map<Publisher, PublisherDTO>(It.IsAny<Publisher>())).Returns(pubDto);

            var result = _publisherService.GetByName(string.Empty, "lang");
            Assert.IsNotNull(result);
        }

        [Test]
        public void RemovePublisher_Revome_SaveWorkNever()
        {
            _publisherService.RemovePublisher(string.Empty);
            _baseService
                .Setup(service => service.LogDelete(It.IsAny<Publisher>()));

            _unitOfWork.Verify(un => un.Save(), Times.Never);
        }

        [Test]
        public void RemovePublisher_TryRevome_SaveWorkNever()
        {
            IEnumerable<Publisher> pub = new List<Publisher>
            {
                new Publisher(),
                new Publisher()
            };

            _unitOfWork.Setup(un => un.PublisherRepository
                    .Get(It.IsAny<Expression<Func<Publisher, bool>>>(),
                        It.IsAny<Expression<Func<Publisher, object>>[]>()))
                .Returns(pub);
            _unitOfWork.Setup(un => un.PublisherRepository.Update(It.IsAny<Publisher>()));
            _baseService
                .Setup(service => service.LogDelete(It.IsAny<Publisher>()));

            _publisherService.RemovePublisher("ME");
            _unitOfWork.Verify(un => un.Save(), Times.Once);
        }

        [Test]
        public void EditPublisher_TestSave_WorksOnce()
        {
            var publisherDto = new PublisherDTO
            {
                PublisherTranslate = new List<PublisherTranslateDto>()
            };
            var publisher = new Publisher
            {
                PublisherTranslate = new List<PublisherTranslate>()
            };
            _unitOfWork.Setup(unit => unit.PublisherRepository
                    .FindById(It.IsAny<int>(),
                        It.IsAny<Expression<Func<Publisher, object>>[]>()))
                .Returns(publisher);
            _unitOfWork.Setup(unit => unit.PublisherRepository.Update(It.IsAny<Publisher>()));
            _baseService.Setup(service => service.LogUpdate(It.IsAny<Publisher>(), It.IsAny<Publisher>()));

            _publisherService.EditPublisher(publisherDto);
            _unitOfWork.Verify(un => un.Save(), Times.Once);
        }
    }
}
