using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;

namespace GameStore.Bll.Strategies
{
    public class VisaStrategy : IStrategy
    {
        public OrderDTO Pay(OrderDTO orderDto)
        {
            return orderDto;
        }
    }
}