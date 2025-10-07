using System;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;

namespace GameStore.Bll.Services
{
    public class PayService : IPayService
    {
        private IStrategy _strategy;

        public PayService()
        {
        }

        public PayService(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public OrderDTO Pay(OrderDTO orderDto)
        {
            if (_strategy != null)
            {
                return _strategy.Pay(orderDto);
            }

            throw new ArgumentNullException(null, "Strategy was not set up!");
        }

        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }
    }
}
