using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Infrastructure;
using GameStore.Bll.Interfaces;
using GameStore.Bll.Observer.Interfaces;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;
using GameStore.Web;
using Moq;
using NUnit.Framework;

namespace GameStore.Bll.Tests.ServiceTests
{
    public class OrderServiceTests
    {
        private readonly Mock<IEventLogger> _baseServiceOrder;
        private readonly Mock<IEventLogger> _baseServiceOrderDetails;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ISenderService> _observable;
        private OrderService _ordersService;
        private Mock<IUnitOfWork> _unitOfWork;

        public OrderServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _baseServiceOrder = new Mock<IEventLogger>();
            _baseServiceOrderDetails = new Mock<IEventLogger>();
            _baseServiceOrder = new Mock<IEventLogger>();
            _baseServiceOrderDetails = new Mock<IEventLogger>();
            _observable = new Mock<ISenderService>();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _ordersService = new OrderService(_unitOfWork.Object,
                _baseServiceOrder.Object,
                _baseServiceOrderDetails.Object,
                _observable.Object);
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutomapperWebProfile>();
                cfg.AddProfile<MapProfile>();
            });
        }

        [Test]
        public void GetOrder_Getorder_ReturnNull()
        {
            IEnumerable<Order> orders = new List<Order>();
            _unitOfWork.Setup(genr => genr.OrdersRepository.Get(It.IsAny<Func<Order, bool>>())).Returns(orders);

            var result = _ordersService.GetOrder(1);
            Assert.IsNull(result);
        }

        [Test]
        public void GetOrder_Getorder_ReturnOrder()
        {
            IEnumerable<Order> orders = new List<Order>
            {
                new Order
                {
                    OrderDetails = new List<OrderDetails>
                    {
                        new OrderDetails()
                    }
                }
            };
            var genresDto = new OrderDTO();
            _unitOfWork.Setup(genr => genr.OrdersRepository
                .Get(It.IsAny<Func<Order, bool>>(),
                    It.IsAny<Expression<Func<Order, object>>[]>())).Returns(orders);
            _mapper.Setup(mapper => mapper.Map<Order, OrderDTO>(
                It.IsAny<Order>())).Returns(genresDto);

            var result = _ordersService.GetOrder(1);
            Assert.IsNotNull(result);
        }

        [Test]
        public void NewOrder_Getorder_SaveOnce()
        {
            IEnumerable<Order> orders = new List<Order>();
            IEnumerable<Game> games = new List<Game>
            {
                new Game()
            };
            var genresDto = new OrderDTO();
            _unitOfWork.Setup(genr => genr.OrdersRepository.Get(It.IsAny<Func<Order, bool>>())).Returns(orders);
            _unitOfWork.Setup(genr => genr.OrdersRepository.Create(It.IsAny<Order>()));
            _unitOfWork.Setup(g => g.GameRepository
                .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                    It.IsAny<Expression<Func<Game, object>>[]>())).Returns(games);
            _baseServiceOrder
                .Setup(service => service.LogCreate(It.IsAny<Order>()));
            _baseServiceOrderDetails
                .Setup(service => service.LogCreate(It.IsAny<OrderDetails>()));
            _baseServiceOrderDetails
                .Setup(service => service.LogUpdate(It.IsAny<OrderDetails>(), It.IsAny<OrderDetails>()));

            _ordersService.NewOrder("Key", 1);
            _unitOfWork.Verify(create => create.Save(), Times.Once);
        }

        [Test]
        public void NewOrder_Getorder_SaveTwice()
        {
            IEnumerable<Order> orders = new List<Order>();
            IEnumerable<Game> games = new List<Game>
            {
                new Game
                {
                    UnitInStock = 5
                }
            };
            _unitOfWork.Setup(genr => genr.OrdersRepository.Get(It.IsAny<Func<Order, bool>>())).Returns(orders);
            _unitOfWork.Setup(genr => genr.OrdersRepository.Create(It.IsAny<Order>()));
            _unitOfWork.Setup(genr => genr.OrdersRepository.Update(It.IsAny<Order>()));
            _unitOfWork.Setup(genr => genr.OrderDetailsRepository.Create(It.IsAny<OrderDetails>()));
            _unitOfWork.Setup(g => g.GameRepository
                .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                    It.IsAny<Expression<Func<Game, object>>[]>())).Returns(games);
            _unitOfWork.Setup(g => g.GameRepository.Update(It.IsAny<Game>()));
            _baseServiceOrder
                .Setup(service => service.LogCreate(It.IsAny<Order>()));
            _baseServiceOrderDetails
                .Setup(service => service.LogCreate(It.IsAny<OrderDetails>()));
            _baseServiceOrderDetails
                .Setup(service => service.LogUpdate(It.IsAny<OrderDetails>(), It.IsAny<OrderDetails>()));

            _ordersService.NewOrder("Key", 1);
            _unitOfWork.Verify(create => create.Save(), Times.Once);
        }

        [Test]
        public void RemoveOrderDetails_RemoveOrderDetails_SaveNever()
        {
            IEnumerable<OrderDetails> orders = new List<OrderDetails>();
            _unitOfWork.Setup(genr => genr.OrderDetailsRepository
                .Get(It.IsAny<Expression<Func<OrderDetails, bool>>>(),
                    It.IsAny<Expression<Func<OrderDetails, object>>[]>())).Returns(orders);

            _ordersService.RemoveOrderDetails(1);
            _unitOfWork.Verify(create => create.Save(), Times.Never);
        }

        [Test]
        public void RemoveOrderDetails_WayForIf_SaveOnce()
        {
            IEnumerable<OrderDetails> orders = new List<OrderDetails>
            {
                new OrderDetails
                {
                    Quantity = 1
                }
            };
            IEnumerable<Game> games = new List<Game>
            {
                new Game()
            };
            _unitOfWork.Setup(genr => genr.OrderDetailsRepository
                .Get(It.IsAny<Expression<Func<OrderDetails, bool>>>(),
                    It.IsAny<Expression<Func<OrderDetails, object>>[]>())).Returns(orders);
            _unitOfWork.Setup(unit => unit.GameRepository
                .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                    It.IsAny<Expression<Func<Game, object>>[]>())).Returns(games);
            _baseServiceOrderDetails
                .Setup(service => service.LogDelete(It.IsAny<OrderDetails>()));

            _ordersService.RemoveOrderDetails(1);
            _unitOfWork.Verify(create => create.Save(), Times.Once);
        }

        [Test]
        public void RemoveOrderDetails_WayForElse_SaveOnce()
        {
            IEnumerable<OrderDetails> orders = new List<OrderDetails>
            {
                new OrderDetails
                {
                    Quantity = 2
                }
            };
            IEnumerable<Game> games = new List<Game>
            {
                new Game()
            };
            _unitOfWork.Setup(genr => genr.OrderDetailsRepository
                .Get(It.IsAny<Expression<Func<OrderDetails, bool>>>(),
                    It.IsAny<Expression<Func<OrderDetails, object>>[]>())).Returns(orders);
            _unitOfWork.Setup(unit => unit.GameRepository
                .Get(It.IsAny<Expression<Func<Game, bool>>>(),
                    It.IsAny<Expression<Func<Game, object>>[]>())).Returns(games);
            _baseServiceOrderDetails
                .Setup(service => service.LogDelete(It.IsAny<OrderDetails>()));

            _ordersService.RemoveOrderDetails(1);
            _unitOfWork.Verify(create => create.Save(), Times.Once);
        }

        [Test]
        public void PayOrder_CheckSave_SaveNever()
        {
            IEnumerable<Order> orders = new List<Order>();
            var ordersDto = new OrderDTO();
            _unitOfWork.Setup(genr => genr.OrdersRepository
                .Get(It.IsAny<Func<Order, bool>>())).Returns(orders);
            _baseServiceOrder
                .Setup(service => service.LogUpdate(It.IsAny<Order>(), It.IsAny<Order>()));

            _ordersService.PayOrder(null);
            _unitOfWork.Verify(create => create.Save(), Times.Never);
        }

        [Test]
        public void PayOrder_CheckSave_SaveOnce()
        {
            IEnumerable<Order> orders = new List<Order>
            {
                new Order()
            };
            var ordersDto = new OrderDTO();
            var order = new Order();
            _unitOfWork.Setup(unit => unit.OrdersRepository
                .Get(It.IsAny<Func<Order, bool>>())).Returns(orders);
            _unitOfWork.Setup(genr => genr.OrdersRepository.FindById(It.IsAny<int>()))
                .Returns(order);

            _ordersService.PayOrder(ordersDto);
            _unitOfWork.Verify(create => create.Save(), Times.Once);
        }

        [Test]
        public void CreateOrderIfNotExists_TestSave_WorksOnce()
        {
            _unitOfWork.Setup(unit => unit.OrdersRepository.Create(It.IsAny<Order>()));
            _ordersService.CreateOrderIfNotExists(1);
            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void CreateOrderDetailsIfNotExists_TestSave_WorksOnce()
        {
            var game = new Game();
            var order = new Order
            {
                OrderDetails = new List<OrderDetails>()
            };
            _unitOfWork.Setup(unit => unit.OrdersRepository.Update(It.IsAny<Order>()));
            _unitOfWork.Setup(unit => unit.GameRepository.Update(It.IsAny<Game>()));
            _ordersService.CreateOrderDetailsIfNotExists(order, game);
            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void GetFromSQL_GetOrders_GetNotNull()
        {
            IEnumerable<Order> orders = new List<Order>();
            _unitOfWork.Setup(unit => unit.OrdersRepository.Get()).Returns(orders);

            var result = _ordersService.GetFromSQL();

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetFromSQL_GetOrders_GetNotEmpty()
        {
            IEnumerable<Order> orders = new List<Order>
            {
                new Order()
            };
            _unitOfWork.Setup(unit => unit.OrdersRepository.Get()).Returns(orders);

            var result = _ordersService.GetFromSQL();

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void SetShipped_TestSave_WorksNever()
        {
            IEnumerable<Order> orders = new List<Order>();
            _unitOfWork.Setup(unit => unit.OrdersRepository.Get(It.IsAny<Func<Order, bool>>()))
                .Returns(orders);

            _ordersService.SetShipped(1);

            _unitOfWork.Verify(unit => unit.Save(), Times.Never);
        }

        [Test]
        public void SetShipped_TestSave_WorksOnce()
        {
            IEnumerable<Order> orders = new List<Order>
            {
                new Order()
            };
            _unitOfWork.Setup(unit => unit.OrdersRepository.Get(It.IsAny<Func<Order, bool>>()))
                .Returns(orders);
            _unitOfWork.Setup(unit => unit.OrdersRepository.Update(It.IsAny<Order>()));
            _ordersService.SetShipped(1);

            _unitOfWork.Verify(unit => unit.Save(), Times.Once);
        }

        [Test]
        public void ReturnInRangeFromSQL_Getorders_GetNotNull()
        {
            IEnumerable<Order> orders = new List<Order>
            {
                new Order()
            };
            _unitOfWork.Setup(unit => unit.OrdersRepository.Get(It.IsAny<Func<Order, bool>>()))
                .Returns(orders);

            var result = _ordersService.ReturnInRangeFromSQL(It.IsAny<DateTime>(), It.IsAny<DateTime>());

            Assert.IsNotNull(result);
        }

        [Test]
        public void ReturnInRangeFromSQL_Getorders_GetNotEmpty()
        {
            IEnumerable<Order> orders = new List<Order>
            {
                new Order()
            };
            _unitOfWork.Setup(unit => unit.OrdersRepository.Get(It.IsAny<Func<Order, bool>>()))
                .Returns(orders);

            var result = _ordersService.ReturnInRangeFromSQL(It.IsAny<DateTime>(), It.IsAny<DateTime>());

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void NewOrderByGameId_Getorder_SaveOnce()
        {
            IEnumerable<Order> orders = new List<Order>();
            IEnumerable<Game> games = new List<Game>
            {
                new Game()
            };
            var genresDto = new OrderDTO();
            _unitOfWork.Setup(genr => genr.OrdersRepository.Get(It.IsAny<Func<Order, bool>>())).Returns(orders);
            _unitOfWork.Setup(genr => genr.OrdersRepository.Create(It.IsAny<Order>()));
            _unitOfWork.Setup(g => g.GameRepository.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(games);
            _baseServiceOrder
                .Setup(service => service.LogCreate(It.IsAny<Order>()));
            _baseServiceOrderDetails
                .Setup(service => service.LogCreate(It.IsAny<OrderDetails>()));
            _baseServiceOrderDetails
                .Setup(service => service.LogUpdate(It.IsAny<OrderDetails>(), It.IsAny<OrderDetails>()));

            _ordersService.NewOrderByGameId(1, 1);
            _unitOfWork.Verify(create => create.Save(), Times.Once);
        }

        [Test]
        public void NewOrderByGameId_Getorder_SaveTwice()
        {
            IEnumerable<Order> orders = new List<Order>();
            IEnumerable<Game> games = new List<Game>
            {
                new Game
                {
                    UnitInStock = 5
                }
            };
            _unitOfWork.Setup(genr => genr.OrdersRepository.Get(It.IsAny<Func<Order, bool>>())).Returns(orders);
            _unitOfWork.Setup(genr => genr.OrdersRepository.Create(It.IsAny<Order>()));
            _unitOfWork.Setup(genr => genr.OrdersRepository.Update(It.IsAny<Order>()));
            _unitOfWork.Setup(genr => genr.OrderDetailsRepository.Create(It.IsAny<OrderDetails>()));
            _unitOfWork.Setup(g => g.GameRepository.Get(It.IsAny<Expression<Func<Game, bool>>>())).Returns(games);
            _unitOfWork.Setup(g => g.GameRepository.Update(It.IsAny<Game>()));
            _baseServiceOrder
                .Setup(service => service.LogCreate(It.IsAny<Order>()));
            _baseServiceOrderDetails
                .Setup(service => service.LogCreate(It.IsAny<OrderDetails>()));
            _baseServiceOrderDetails
                .Setup(service => service.LogUpdate(It.IsAny<OrderDetails>(), It.IsAny<OrderDetails>()));

            _ordersService.NewOrderByGameId(1, 1);
            _unitOfWork.Verify(create => create.Save(), Times.Once);
        }

        [Test]
        public void GetAll_TestValue_GetNotNull()
        {
            _unitOfWork.Setup(unit => unit.OrdersRepository.Get()).Returns(new List<Order>());

            var result = _ordersService.GetAll();

            Assert.IsNotNull(result);
        }
        [Test]
        public void GetAll_TestValue_GetNotEmpty()
        {
            IEnumerable<Order> ordersList = new List<Order>
            {
                new Order()
            };
            _unitOfWork.Setup(unit => unit.OrdersRepository.Get()).Returns(ordersList);

            var result = _ordersService.GetAll();

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void ReturnInRange_TestValue_GetNotNull()
        {
            _unitOfWork.Setup(unit => unit.OrdersRepository
                .Get(It.IsAny<Func<Order, bool>>())).Returns(new List<Order>());

            var result = _ordersService.ReturnInRange(DateTime.Now, DateTime.Now);

            Assert.IsNotNull(result);
        }

        [Test]
        public void ReturnInRange_TestValue_GetNotEmpty()
        {
            IEnumerable<Order> ordersList = new List<Order>
            {
                new Order()
            };
            _unitOfWork.Setup(unit => unit.OrdersRepository
                .Get(It.IsAny<Func<Order, bool>>())).Returns(ordersList);

            var result = _ordersService.ReturnInRange(DateTime.Now, DateTime.Now);

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetOrderByUserEmail_TestValue_GetNull()
        {
            _unitOfWork.Setup(unit => unit.UserRepository
                .Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>());
            _unitOfWork.Setup(unit => unit.OrdersRepository
                .Get(It.IsAny<Expression<Func<Order, bool>>>())).Returns(It.IsAny<IEnumerable<Order>>());

            var result = _ordersService.GetOrderByUserEmail("ME");

            Assert.IsNull(result);
        }

        [Test]
        public void GetOrderByUserEmail_TestValue_GetNotNull()
        {
            IEnumerable<Order> ordersList = new List<Order>
            {
                new Order
                {
                    OrderDetails = new List<OrderDetails>
                    {
                        new OrderDetails()
                    }
                }
            };

            _unitOfWork.Setup(unit => unit.UserRepository
                .Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(new List<User>());
            _unitOfWork.Setup(unit => unit.OrdersRepository
                .Get(It.IsAny<Func<Order,bool>>(),
                    It.IsAny<Expression<Func<Order, object>>[]>())).Returns(ordersList);

            var result = _ordersService.GetOrderByUserEmail("ME");

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetOrderById_TestValue_GetNull()
        {
            _unitOfWork.Setup(unit => unit.OrdersRepository
                .Get(It.IsAny<Expression<Func<Order, bool>>>())).Returns(It.IsAny<IEnumerable<Order>>());

            var result = _ordersService.GetOrderById(1);

            Assert.IsNull(result);
        }

        [Test]
        public void GetOrderById_TestValue_GetNotNull()
        {
            IEnumerable<Order> ordersList = new List<Order>
            {
                new Order
                {
                    OrderDetails = new List<OrderDetails>
                    {
                        new OrderDetails()
                    }
                }
            };

            _unitOfWork.Setup(unit => unit.OrdersRepository
                .Get(It.IsAny<Func<Order, bool>>(),
                    It.IsAny<Expression<Func<Order, object>>[]>())).Returns(ordersList);

            var result = _ordersService.GetOrderById(1);

            Assert.IsNotNull(result);
        }
    }
}
