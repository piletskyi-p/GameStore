using System;
using System.Collections.Generic;
using GameStore.Bll.DTO;
using GameStore.Bll.Strategies;
using NUnit.Framework;

namespace GameStore.Bll.Tests.StrategiesTests
{
    public class IBoxStrategyTests
    {
        private readonly IBoxStrategy _boxStrategy;

        public IBoxStrategyTests()
        {
            _boxStrategy = new IBoxStrategy();
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

            var res = _boxStrategy.Pay(orderDto);
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

            var res = _boxStrategy.Pay(orderDto);
            Assert.IsNotNull(res); 
        }
    }
}