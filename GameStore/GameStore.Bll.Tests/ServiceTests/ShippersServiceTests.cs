using System.Collections.Generic;
using AutoMapper;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Services;
using GameStore.Dal.Entities.Mongo;
using GameStore.Dal.Interfaces;
using GameStore.Web;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServiceTests
{
    public class ShippersServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private ShippersService _shipperService;

        public ShippersServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
        }

        [SetUp]
        public void Setup()
        {
            _shipperService = new ShippersService( _unitOfWork.Object);
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutomapperWebProfile>();
                cfg.AddProfile<MapProfile>();
            });
        }

        [Test]
        public void Get_TestValue_GetNotNull()
        {
            _unitOfWork.Setup(unit => unit.MongoShippersRepository.Get())
                .Returns(new List<Shippers>());

            var result = _shipperService.Get();

            Assert.IsNotNull(result);
        }

        [Test]
        public void Get_TestValue_GetNotEmptyList()
        {
            IEnumerable<Shippers> shipersList = new List<Shippers>
            {
                new Shippers()
            };
            _unitOfWork.Setup(unit => unit.MongoShippersRepository.Get())
                .Returns(shipersList);

            var result = _shipperService.Get();
       
            Assert.IsNotEmpty(result);
        }
    }
}