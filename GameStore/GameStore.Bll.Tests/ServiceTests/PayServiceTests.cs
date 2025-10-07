using System;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Interfaces;
using GameStore.Bll.Services;
using GameStore.Web;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServiceTests
{
    public class PayServiceTests
    {
        private Mock<IStrategy> _stratagy;
        private PayService _payService;

        [SetUp]
        public void Setup()
        {
            _stratagy = new Mock<IStrategy>();

            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutomapperWebProfile>();
                cfg.AddProfile<MapProfile>();
            });
        }

        [Test]
        public void Pay_Test_GetNotNull()
        {
            _payService = new PayService(_stratagy.Object);
            var orderDto = new OrderDTO();
            _stratagy.Setup(str => str.Pay(It.IsAny<OrderDTO>()))
                .Returns(orderDto);

            var result = _payService.Pay(orderDto);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Pay_Test_GetError()
        {
            _payService = new PayService(null);
            var orderDto = new OrderDTO();

            var ex = Assert.Throws<ArgumentNullException>(() => _payService.Pay(orderDto));
            Assert.That(ex.Message, Is.EqualTo("Strategy was not set up!"));
        }
    }
}