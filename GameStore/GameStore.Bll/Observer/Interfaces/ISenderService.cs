namespace GameStore.Bll.Observer.Interfaces
{
    public interface ISenderService
    {
        void NotifyObservers(object info);
        void SentInfo(OrderModel order);
        OrderModel GetOrder(object orderObject);
    }
}