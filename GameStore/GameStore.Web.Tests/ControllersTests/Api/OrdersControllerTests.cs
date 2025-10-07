using System.Net;
using System.Web.Http.Results;
using AutoMapper;
using GameStore.Bll.Interfaces;
using GameStore.Web.Controllers.Api;
using Moq;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests.Api
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _orderService;
        private readonly Mock<IUserService> _userService;
        private OrdersController _ordersController;

        public OrdersControllerTests()
        {
            _orderService = new Mock<IOrderService>();
            _userService = new Mock<IUserService>();
        }

        [SetUp]
        public void Setup()
        {
            _ordersController = new OrdersController(
                _orderService.Object,
                _userService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void Delete_TestReturn_GetCorrectData()
        {
            _orderService.Setup(service => service.RemoveOrderDetails(It.IsAny<int>()));

            var result = _ordersController.Delete(1);
            var contentResult = result as NegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }
    }
}