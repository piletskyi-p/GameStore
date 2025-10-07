using System;
using System.Collections.Generic;
using GameStore.Bll.DTO;
using GameStore.Bll.Strategies;
using NUnit.Framework;

namespace GameStore.Bll.Tests.StrategiesTests
{
    public class VisaStrategyTests
    {
        private readonly VisaStrategy _visaStrategy;

        public VisaStrategyTests()
        {
            _visaStrategy = new VisaStrategy();
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

            var res = _visaStrategy.Pay(orderDto);
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

            var res = _visaStrategy.Pay(orderDto);
            Assert.IsNotNull(res); 
        }
    }
}
