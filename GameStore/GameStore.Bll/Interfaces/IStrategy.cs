using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface IStrategy
    {
        OrderDTO Pay(OrderDTO orderJson);
    }
}
