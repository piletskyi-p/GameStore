namespace GameStore.Bll.Observer
{
    public abstract class BaseSender
    {
        public abstract void Send(Manager manager, UserInfo userInfo, OrderModel order);
        
    }
}