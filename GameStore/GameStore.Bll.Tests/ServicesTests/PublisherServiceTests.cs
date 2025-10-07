using System;
using System.Collections.Generic;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using GameStore.Web;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests
{
    public class PublisherServiceTests
    {
        private PublisherService _publisherService;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _publisherService = new PublisherService(_unitOfWork.Object);
            _mapper = new Mock<IMapper>();

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

            var result = _publisherService.GetAllPublisher();
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetById_Get_GetNull()
        {
            var pub = new List<Publisher>
            {
                new Publisher()
            };
            _unitOfWork.Setup(p => p.PublisherRepository.Get(It.IsAny<Func<Publisher, bool>>())).Returns(pub);

            var result = _publisherService.GetById(1);
            Assert.AreEqual(typeof(PublisherDTO), result.GetType());
        }

        [Test]
        public void GetById_Get_GetNotNull()
        {
            var pub = new List<Publisher>
            {
                new Publisher()
            };
            var pubDto = new PublisherDTO();
            _unitOfWork.Setup(p => p.PublisherRepository.Get(It.IsAny<Func<Publisher, bool>>())).Returns(pub);
            _mapper.Setup(mapper => mapper.Map<Publisher, PublisherDTO>(It.IsAny<Publisher>())).Returns(pubDto);

            var result = _publisherService.GetById(1);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetByName_Get_GetNull()
        {
            var pub = new List<Publisher>
            {
                new Publisher()
            };
            _unitOfWork.Setup(p => p.PublisherRepository.Get(It.IsAny<Func<Publisher, bool>>())).Returns(pub);

            var result = _publisherService.GetByName(string.Empty);
            Assert.AreEqual(typeof(PublisherDTO), result.GetType());
        }

        [Test]
        public void GetByName_Get_GetNotNull()
        {
            var pub = new List<Publisher>
            {
                new Publisher()
            };
            var pubDto = new PublisherDTO();
            _unitOfWork.Setup(p => p.PublisherRepository.Get(It.IsAny<Func<Publisher, bool>>())).Returns(pub);
            _mapper.Setup(mapper => mapper.Map<Publisher, PublisherDTO>(It.IsAny<Publisher>())).Returns(pubDto);

            var result = _publisherService.GetByName(string.Empty);
            Assert.IsNotNull(result);
        }

        [Test]
        public void RemovePublisher_Revome_SaveWorkNever()
        {
             _publisherService.RemovePublisher(string.Empty);
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
       
            _unitOfWork.Setup(un => un.PublisherRepository.Get(It.IsAny<Func<Publisher, bool>>()))
                .Returns(pub);
            _unitOfWork.Setup(un => un.PublisherRepository.Update(It.IsAny<Publisher>()));

            _publisherService.RemovePublisher("ME");
            _unitOfWork.Verify(un => un.Save(), Times.Once);
        }
    }
}
