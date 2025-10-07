using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Bll.Observer.Interfaces;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISenderService _observable;
        private readonly IEventLogger _baseServiceOrder;
        private readonly IEventLogger _baseServiceOrderDetails;

        public OrderService(
            IUnitOfWork unitOfWork,
            IEventLogger baseServiceOrder,
            IEventLogger baseServiceOrderDetails,
            ISenderService observable)
        {
            _unitOfWork = unitOfWork;
            _observable = observable;
            _baseServiceOrder = baseServiceOrder;
            _baseServiceOrderDetails = baseServiceOrderDetails;
        }

        public IEnumerable<OrderDTO> GetAll()
        {
            var orders = _unitOfWork.OrdersRepository.Get();
            var orderDto = Mapper.Map<IEnumerable<OrderDTO>>(orders);

            return orderDto;
        }

        public IEnumerable<OrderDTO> ReturnInRange(DateTime from, DateTime to)
        {
            var orders = _unitOfWork.OrdersRepository
                .Get(order => order.OrderDate > from && order.OrderDate < to).ToList();
            var orderSqlDto = Mapper.Map<List<OrderDTO>>(orders);

            return orderSqlDto;
        }

        public IEnumerable<OrderDTO> ReturnInRangeFromSQL(DateTime from, DateTime to)
        {
            var ordersSql = _unitOfWork.OrdersRepository
                .Get(order => order.OrderDate > from && order.OrderDate < to
                              && order.IsPaid).ToList();
            var orderSqlDto = Mapper.Map<List<OrderDTO>>(ordersSql);

            return orderSqlDto;
        }

        public OrderDTO GetOrder(int userId)
        {
            var orders = _unitOfWork.OrdersRepository.Get(user => user.CustomerId == userId.ToString(),
                repository => repository.OrderDetails).Where(paid => paid.IsPaid == false);
            var order = orders.FirstOrDefault();

            if (order != null)
            {
                if (order.OrderDetails.Count != 0)
                {
                    var orderDto = Mapper.Map<Order, OrderDTO>(order);

                    return orderDto;
                }
            }

            return null;
        }

        public OrderDTO GetOrderByUserEmail(string email)
        {
            var user = _unitOfWork.UserRepository
                .Get(us => us.Email == email).FirstOrDefault();
            var order = _unitOfWork.OrdersRepository
                .Get(orders => user != null && orders.CustomerId == user.Id.ToString())
                .FirstOrDefault(paid => paid.IsPaid == false);

            if (order != null)
            {
                if (order.OrderDetails.Count != 0)
                {
                    var orderDto = Mapper.Map<Order, OrderDTO>(order);

                    return orderDto;
                }
            }

            return null;
        }

        public OrderDTO GetOrderById(int id)
        {
            var orders = _unitOfWork.OrdersRepository.Get(ord => ord.Id == id);
            var order = orders.FirstOrDefault();

            if (order != null)
            {
                if (order.OrderDetails.Count != 0)
                {
                    var orderDto = Mapper.Map<Order, OrderDTO>(order);

                    return orderDto;
                }
            }

            return null;
        }

        public void RemoveOrderDetails(int orderDetailsId)
        {
            var orderDetail = _unitOfWork.OrderDetailsRepository.Get(order => order.Id == orderDetailsId).FirstOrDefault();

            if (orderDetail != null)
            {
                var game = _unitOfWork.GameRepository.Get(g => g.Key == orderDetail.ProductID).FirstOrDefault();

                if (game != null)
                {
                    if (orderDetail.Quantity == 1)
                    {
                        _unitOfWork.OrderDetailsRepository.HardDelete(orderDetail);
                        game.UnitInStock++;
                        _unitOfWork.GameRepository.Update(game);
                        _unitOfWork.Save();
                        _baseServiceOrderDetails.LogDelete(orderDetail);
                    }
                    else
                    {
                        orderDetail.Quantity--;
                        _unitOfWork.OrderDetailsRepository.Update(orderDetail);
                        game.UnitInStock++;
                        _unitOfWork.GameRepository.Update(game);
                        _unitOfWork.Save();
                        _baseServiceOrderDetails.LogDelete(orderDetail);
                    }
                }
            }
        }

        public void CreateOrderIfNotExists(int userId)
        {
            Order order = new Order
            {
                CustomerId = userId.ToString(),
                OrderDate = DateTime.UtcNow,
                OrderDetails = new List<OrderDetails>(),
                ShippedDate = DateTime.UtcNow
            };

            _unitOfWork.OrdersRepository.Create(order);
            _unitOfWork.Save();
        }

        public void CreateOrderDetailsIfNotExists(Order order, Game game)
        {
            var orderDetails = new OrderDetails
            {
                ProductID = game.Key,
                Price = game.Price,
                Quantity = 1,
                Discount = game.IsDiscontinued ? 0.1 : 0,
                OrderId = order.Id
            };

            order.OrderDetails.Add(orderDetails);
            _unitOfWork.OrdersRepository.Update(order);

            game.UnitInStock--;
            _unitOfWork.GameRepository.Update(game);
            _unitOfWork.Save();
        }

        public void NewOrder(string key, int userId)
        {
            var temp = _unitOfWork.OrdersRepository.Get(user => user.CustomerId == userId.ToString() && user.IsPaid == false);
            var orderCheck = temp.FirstOrDefault();

            if (orderCheck == null)
            {
                CreateOrderIfNotExists(userId);
            }

            var order = _unitOfWork.OrdersRepository
                .Get(orders => orders.CustomerId == userId.ToString() && orders.IsPaid == false)
                .FirstOrDefault();
            var game = _unitOfWork.GameRepository
                .Get(g => g.Key == key)
                .FirstOrDefault();

            if (game == null || game.UnitInStock < 1 || order == null)
            {
                return;
            }

            var checkExistItem = order.OrderDetails.FirstOrDefault(od => od.ProductID == game.Key);
            if (checkExistItem == null)
            {
                CreateOrderDetailsIfNotExists(order, game);
            }
            else
            {
                checkExistItem.Quantity++;
                _unitOfWork.OrderDetailsRepository.Update(checkExistItem);
            }

            game.UnitInStock--;
            _unitOfWork.GameRepository.Update(game);
            _unitOfWork.Save();
        }

        public void PayOrder(OrderDTO orderDto)
        {
            var order = _unitOfWork.OrdersRepository.Get(ord => ord.Id == orderDto.Id).FirstOrDefault();
            if (order != null)
            {
                var orderOld = new Order
                {
                    Id = order.Id,
                    IsPaid = order.IsPaid,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    OrderDetails = order.OrderDetails
                };

                order.IsPaid = true;
                _unitOfWork.OrdersRepository.Update(order);
                _unitOfWork.Save();

                _baseServiceOrder.LogUpdate(orderOld, order);
                _observable.NotifyObservers(order);
            }
        }

        public IEnumerable<OrderDTO> GetFromSQL()
        {
            var orders = _unitOfWork.OrdersRepository.Get();
            var ordersDto = Mapper.Map<IEnumerable<OrderDTO>>(orders);

            return ordersDto;
        }

        public void SetShipped(int id)
        {
            var order = _unitOfWork.OrdersRepository
                .Get(orders => orders.Id == id).FirstOrDefault();
            if (order != null)
            {
                order.IsShipped = true;
                order.ShippedDate = DateTime.UtcNow;
                _unitOfWork.OrdersRepository.Update(order);
                _unitOfWork.Save();
            }
        }

        public void NewOrderForUser(int userId)
        {
            var temp = _unitOfWork.OrdersRepository.Get(user => user.CustomerId == userId.ToString() && user.IsPaid == false);
            var orderCheck = temp.FirstOrDefault();

            if (orderCheck == null)
            {
                CreateOrderIfNotExists(userId);
            }
        }

        public void NewOrderByGameId(int gameId, int userId)
        {
            var temp = _unitOfWork.OrdersRepository.Get(user => user.CustomerId == userId.ToString() && user.IsPaid == false);
            var orderCheck = temp.FirstOrDefault();

            if (orderCheck == null)
            {
                CreateOrderIfNotExists(userId);
            }

            var order = _unitOfWork.OrdersRepository
                .Get(orders => orders.CustomerId == userId.ToString() && orders.IsPaid == false)
                .FirstOrDefault();
            var game = _unitOfWork.GameRepository
                .Get(g => g.Id == gameId)
                .FirstOrDefault();

            if (game == null || game.UnitInStock < 1 || order == null)
            {
                return;
            }

            var checkExistItem = order.OrderDetails.FirstOrDefault(od => od.ProductID == game.Key);
            if (checkExistItem == null)
            {
                CreateOrderDetailsIfNotExists(order, game);
            }
            else
            {
                checkExistItem.Quantity++;
                _unitOfWork.OrderDetailsRepository.Update(checkExistItem);
            }

            game.UnitInStock--;
            _unitOfWork.GameRepository.Update(game);
            _unitOfWork.Save();
        }
    }
}