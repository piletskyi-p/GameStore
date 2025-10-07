using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Bll.Observer.Interfaces;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Enum;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Observer
{
    public class SenderService : ISenderService
    {
        private readonly IObserver _observer;

        private readonly IUnitOfWork _unitOfWork;

        private ConcurrentQueue<OrderModel> _queueOrders;

        private static readonly Dictionary<Enum, BaseSender> TypeCollectionMap
            = new Dictionary<Enum, BaseSender>
            {
                [Sender.Email] = new EmailSender(),
                [Sender.Mobile] = new MobileSender()
            };

        public SenderService(IObserver observer, IUnitOfWork unitOfWork)
        {
            _observer = observer;
            _unitOfWork = unitOfWork;
            _queueOrders = new ConcurrentQueue<OrderModel>();
        }

        public void NotifyObservers(object order)
        {
            var orderModel = GetOrder(order);
            _queueOrders.Enqueue(orderModel);

            if (_queueOrders.TryDequeue(out var currentOrder))
            {
                Task.Run(() => SentInfo(currentOrder));
            }
        }

        public void SentInfo(OrderModel order)
        {
            var user = _unitOfWork.UserRepository.FindById(order.CustomerId);
            var userInfo = Mapper.Map<UserInfo>(user);
            var observers = _observer.GetObservers();

            foreach (var manager in observers.Managers)
            {
                var obj = TypeCollectionMap[manager.SenderType];
                obj.Send(manager, userInfo, order);
            }
        }

        public OrderModel GetOrder(object orderObject)
        {
            var order = (Order)orderObject;
            var orderModel = Mapper.Map<OrderModel>(order);

            orderModel.Games = new List<Game>();
            foreach (var details in order.OrderDetails)
            {
                orderModel.Games.Add(_unitOfWork.GameRepository
                    .Get(game => game.Key == details.ProductID).FirstOrDefault());
                orderModel.Price += details.Price;
            }

            return orderModel;
        }
    }
}