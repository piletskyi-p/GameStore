using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface IPayService
    {
        OrderDTO Pay(OrderDTO orderJson);
        void SetStrategy(IStrategy strategy);
    }
}
