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
    public class OrderServiceTests
    {
        private OrderService _ordersService;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _ordersService = new OrderService(_unitOfWork.Object);
            _mapper = new Mock<IMapper>();

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
                new Order()
            };
            var genresDto = new OrderDTO();
            _unitOfWork.Setup(genr => genr.OrdersRepository.Get(It.IsAny<Func<Order, bool>>())).Returns(orders);
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
            _unitOfWork.Setup(g => g.GameRepository.Get(It.IsAny<Func<Game, bool>>())).Returns(games);

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
            _unitOfWork.Setup(g => g.GameRepository.Get(It.IsAny<Func<Game, bool>>())).Returns(games);
            _unitOfWork.Setup(g => g.GameRepository.Update(It.IsAny<Game>()));

            _ordersService.NewOrder("Key", 1);
            _unitOfWork.Verify(create => create.Save(), Times.Once); 
        }

        [Test]
        public void GetOrderDetails_Getorder_GetNull()
        {
            IEnumerable<OrderDetails> order = new List<OrderDetails>();
             _unitOfWork.Setup(genr => genr.OrderDetailsRepository
                 .Get(It.IsAny<Func<OrderDetails, bool>>()))
                 .Returns(order);

            var result = _ordersService.GetOrderDetails(1);
            Assert.IsNull(result);
        }

        [Test]
        public void GetOrderDetails_Getorder_GetNotNull()
        {
            IEnumerable<OrderDetails> orders = new List<OrderDetails>
            {
                new OrderDetails()
            };

            OrderDetailsDTO ordersDto = new OrderDetailsDTO();

            _unitOfWork.Setup(genr => genr.OrderDetailsRepository
                .Get(It.IsAny<Func<OrderDetails, bool>>()))
                .Returns(orders);
            _mapper.Setup(mapper => mapper.Map<OrderDetails, OrderDetailsDTO>(
                It.IsAny<OrderDetails>())).Returns(ordersDto);

            var result = _ordersService.GetOrderDetails(1);
            Assert.IsNotNull(result);
        }

        [Test]
        public void RemoveOrderDetails_TestSave_SaveNever()
        {
            _unitOfWork.Setup(genr => genr.OrderDetailsRepository
                    .Get(It.IsAny<Func<OrderDetails, bool>>()))
                .Returns(It.IsAny<IEnumerable<OrderDetails>>());

            _unitOfWork.Verify(create => create.Save(), Times.Never);
        }

        [Test]
        public void RemoveOrderDetails_FirstWay_SaveOnce()
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

            OrderDetailsDTO ordersDto = new OrderDetailsDTO();

            _unitOfWork.Setup(genr => genr.OrderDetailsRepository
                    .Get(It.IsAny<Func<OrderDetails, bool>>()))
                .Returns(orders);
            _unitOfWork.Setup(game => game.GameRepository
                    .Get(It.IsAny<Func<Game, bool>>()))
                .Returns(games);

            _ordersService.RemoveOrderDetails(1);
            _unitOfWork.Verify(create => create.Save(), Times.Once);
        }

        [Test]
        public void RemoveOrderDetails_SecondWay_SaveOnce()
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

            OrderDetailsDTO ordersDto = new OrderDetailsDTO();

            _unitOfWork.Setup(genr => genr.OrderDetailsRepository
                    .Get(It.IsAny<Func<OrderDetails, bool>>()))
                .Returns(orders);
            _unitOfWork.Setup(game => game.GameRepository
                    .Get(It.IsAny<Func<Game, bool>>()))
                .Returns(games);

            _ordersService.RemoveOrderDetails(1);
            _unitOfWork.Verify(create => create.Save(), Times.Once);
        }
    }
}
