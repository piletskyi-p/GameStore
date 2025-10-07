using System;
using System.Collections.Generic;
using GameStore.Bll.Strategies;
using GameStore.Bll.DTO;
using NUnit.Framework;

namespace GameStore.Bll.Tests.StrategiesTests
{
    public class BankStrategyTests
    {
        private readonly BankStrategy _bankStrategy;

        public BankStrategyTests()
        {
            _bankStrategy = new BankStrategy();
        }

        [Test]
        public void Pay_Pay_ReturnCorrectType()
        {
            var orderDto = new OrderDTO
            {
                CustomerId = "1",
                OrderDate = DateTime.UtcNow,
                OrderDetails = new List<OrderDetailsDTO>()
            };

            var res = _bankStrategy.Pay(orderDto);
            Assert.AreEqual(typeof(OrderDTO), res.GetType());
        }

        [Test]
        public void Pay_Pay_ReturnNotNull()
        {
            var orderDto = new OrderDTO
            {
                CustomerId = "1",
                OrderDate = DateTime.UtcNow,
                OrderDetails = new List<OrderDetailsDTO>()
            };

            var res = _bankStrategy.Pay(orderDto);
            Assert.IsNotNull(res);
        }
    }
}
