using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Web.Auth;
using GameStore.Web.Controllers;
using Moq;
using Newtonsoft.Json;
using NLog;
using NUnit.Framework;

namespace GameStore.Web.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _ordersService;
        private readonly Mock<IPayService> _payService;
        private readonly Mock<IGameService> _gameService;
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IShippersService> _shippedService;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IAuthentication> _auth;
        private readonly Mock<ILanguageService> _languageService;
        private OrderController _orderController;

        public OrderControllerTests()
        {
            _ordersService = new Mock<IOrderService>();
            _payService = new Mock<IPayService>();
            _gameService = new Mock<IGameService>();
            _userService = new Mock<IUserService>();
            _logger = new Mock<ILogger>();
            _auth = new Mock<IAuthentication>();
            _languageService = new Mock<ILanguageService>();
            _shippedService = new Mock<IShippersService>();
        }

        [SetUp]
        public void Setup()
        {
            _orderController = new OrderController(
                _ordersService.Object,
                _payService.Object,
                _gameService.Object,
                _logger.Object,
                _userService.Object,
                _shippedService.Object,
                _auth.Object,
                _languageService.Object);

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperWebProfile>());
        }

        [Test]
        public void Buy_BuyGame_ReturnEx()
        {
            var result = _orderController.Buy(string.Empty);
            var ex = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void Buy_BuyGame_ReturnExOrderNull()
        {
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(It.IsAny<OrderDTO>());
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = _orderController.Buy("ME");
            var ex = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void Buy_BuyGame_ReturnNotNullModel()
        {
            var ordersDto = new OrderDTO();
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(ordersDto);
            _ordersService.Setup(order => order.NewOrder(It.IsAny<string>(), It.IsAny<int>()));
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = _orderController.Buy("ME");
            Assert.IsNotNull(result);
        }

        [Test]
        public void Buy_BuyGame_CorrectParamForRedirect()
        {
            var ordersDto = new OrderDTO();
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(ordersDto);
            _ordersService.Setup(order => order.NewOrder(It.IsAny<string>(), It.IsAny<int>()));
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = (RedirectToRouteResult)_orderController.Buy("ME");
            Assert.AreEqual(3, result.RouteValues.Values.Count);
        }

        [Test]
        public void Basket_ShowBasket_ReturnCorrectView()
        {
            var ordersDto = new OrderDTO();
            _ordersService.Setup(order => order
                .GetOrder(It.IsAny<int>())).Returns(ordersDto);
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = (ViewResult)_orderController.Basket();
            Assert.AreEqual("Basket", result.ViewName);
        }

        [Test]
        public void Basket_ShowBasket_ReturnNotNull()
        {
            var ordersDto = new OrderDTO();
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(ordersDto);
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = (ViewResult)_orderController.Basket();
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void Order_GetOrder_ReturnEx()
        {
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(It.IsAny<OrderDTO>());
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = _orderController.Order();
            var ex = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            Assert.AreEqual(ex.GetType(), result.GetType());
        }

        [Test]
        public void Order_GetOrder_ReturnNotNullModel()
        {
            var orderDro = new OrderDTO();
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(orderDro);
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = (ViewResult)_orderController.Order();
            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void Order_GetOrder_ReturnCorrectView()
        {
            var orderDro = new OrderDTO();
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(orderDro);
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = (ViewResult)_orderController.Order();
            Assert.AreEqual("Order", result.ViewName);
        }

        [Test]
        public void PayBank_GetFile_ReturnCorrectType()
        {
            var orderDto = new OrderDTO
            {
                OrderDetails = new List<OrderDetailsDTO>()
            };
            var games = new List<GameDTO>();
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(orderDto);
            _gameService.Setup(game => game.GetAllGames()).Returns(games);
            _payService.Setup(pay => pay.Pay(It.IsAny<OrderDTO>())).Returns(orderDto);
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            MemoryStream Stream = new MemoryStream();
            var file = new FileStreamResult(Stream, "ME");

            var result = _orderController.PayBank();

            Assert.AreEqual(file.GetType(), result.GetType());
        }

        [Test]
        public void PayBank_GetFile_ReturnNotNull()
        {
            var orderDto = new OrderDTO
            {
                OrderDetails = new List<OrderDetailsDTO>()
            };
            var games = new List<GameDTO>();
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(orderDto);
            _gameService.Setup(game => game.GetAllGames()).Returns(games);
            _payService.Setup(pay => pay.Pay(It.IsAny<OrderDTO>())).Returns(orderDto);
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = _orderController.PayBank();

            Assert.IsNotNull(result);
        }

        [Test]
        public void IBox_GetFile_ReturCorrectType()
        {
            var orderDro = new OrderDTO
            {
                OrderDetails = new List<OrderDetailsDTO>()
            };
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(orderDro);
            _payService.Setup(pay => pay.Pay(It.IsAny<OrderDTO>())).Returns(orderDro);
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = (ViewResult)_orderController.IBox();
            Assert.AreEqual(typeof(OrderDTO), result.Model.GetType());
        }

        [Test]
        public void IBox_GetFile_ReturCorrectView()
        {
            var orderDro = new OrderDTO
            {
                OrderDetails = new List<OrderDetailsDTO>()
            };
            var orderDrojson = JsonConvert.SerializeObject(orderDro);
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(orderDro);
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = (ViewResult)_orderController.IBox();

            Assert.AreEqual("IBox", result.ViewName);
        }

        [Test]
        public void Congratulation_GetFile_ReturCorrectView()
        {
            var result = (ViewResult)_orderController.Congratulation();

            Assert.AreEqual("Congratulation", result.ViewName);
        }

        [Test]
        public void PayVisa_GetFile_ReturCorrectView()
        {
            _ordersService.Setup(order => order.GetOrder(It.IsAny<int>())).Returns(It.IsAny<OrderDTO>());
            _userService.Setup(user => user
                .GetUser(It.IsAny<string>())).Returns(new UserDTO());
            _auth.Setup(action => action.CurrentUser.Identity.Name).Returns("email");

            var result = (ViewResult)_orderController.PayVisa();

            Assert.AreEqual("Congratulation", result.ViewName);
        }

        [Test]
        public void VisaView_GetView_ReturCorrectView()
        {
            var result = (ViewResult)_orderController.VisaView();

            Assert.AreEqual("VisaView", result.ViewName);
        }

        [Test]
        public void RemoveFromBasket_GetView_ReturCorrectView()
        {
            var result = (RedirectToRouteResult)_orderController.RemoveFromBasket(1);

            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
        }

        [Test]
        public void GetAllOrders_GetView_ReturCorrectView()
        {
            var result = (ViewResult)_orderController.GetAllOrders(1);

            Assert.AreEqual("AllOrders", result.ViewName);
        }

        [Test]
        public void GetAllOrders_GetView_ReturNotNullModel()
        {
            var result = (ViewResult)_orderController.GetAllOrders(1);

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void OrderFilter_GetViewIfWrong_ReturCorrectView()
        {
            var result = (ViewResult)_orderController.OrderFilter(DateTime.UtcNow.AddDays(1), DateTime.UtcNow);

            Assert.AreEqual("AllOrders", result.ViewName);
        }

        [Test]
        public void OrderFilter_GetViewIfWrong_ReturNotNullModel()
        {
            var result = (ViewResult)_orderController.OrderFilter(DateTime.UtcNow.AddDays(1), DateTime.UtcNow);

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void OrderFilter_GetView_ReturCorrectView()
        {
            var result = (ViewResult)_orderController.OrderFilter(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

            Assert.AreEqual("AllOrders", result.ViewName);
        }

        [Test]
        public void OrderFilter_GetView_ReturNotNullModel()
        {
            var result = (ViewResult)_orderController.OrderFilter(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void GetOrdersFromSql_GetView_ReturCorrectView()
        {
            var result = (ViewResult)_orderController.GetOrdersFromSql();

            Assert.AreEqual("OrdersSQL", result.ViewName);
        }

        [Test]
        public void GetOrdersFromSql_GetView_ReturNotNullModel()
        {
            var result = (ViewResult)_orderController.GetOrdersFromSql();

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void SetShipped_GetView_ExistAction()
        {
            var result = (RedirectToRouteResult)_orderController.SetShipped(1);

            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
        }

        [Test]
        public void OrderFilterSQL_GetViewIfWrong_ReturCorrectView()
        {
            var result = (ViewResult)_orderController
                .OrderFilterSQL(DateTime.UtcNow.AddDays(1), DateTime.UtcNow);

            Assert.AreEqual("AllOrders", result.ViewName);
        }

        [Test]
        public void OrderFilterSQL_GetViewIfWrong_ReturNotNullModel()
        {
            var result = (ViewResult)_orderController
                .OrderFilterSQL(DateTime.UtcNow.AddDays(1), DateTime.UtcNow);

            Assert.IsNotNull(result.Model);
        }

        [Test]
        public void OrderFilterSQL_GetView_ReturCorrectView()
        {
            var list = new List<OrderDTO>
            {
                new OrderDTO()
            };
            _ordersService.Setup(service => service
                    .ReturnInRangeFromSQL(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(list);
            var result = (ViewResult)_orderController
                .OrderFilterSQL(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

            Assert.AreEqual("OrdersSQL", result.ViewName);
        }

        [Test]
        public void OrderFilterSQL_GetView_ReturNotNullModel()
        {
            var result = (ViewResult)_orderController
                .OrderFilterSQL(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

            Assert.IsNotNull(result.Model);
        }
    }
}