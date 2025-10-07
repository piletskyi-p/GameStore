using System;
using System.Collections.Generic;
using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderDTO> GetAll();
        IEnumerable<OrderDTO> GetFromSQL();
        IEnumerable<OrderDTO> ReturnInRange(DateTime from, DateTime to);
        IEnumerable<OrderDTO> ReturnInRangeFromSQL(DateTime from, DateTime to);

        void NewOrder(string key, int userId);
        void NewOrderByGameId(int gameId, int userId);
        void NewOrderForUser(int userId);

        OrderDTO GetOrder(int userId);
        OrderDTO GetOrderById(int Id);
        OrderDTO GetOrderByUserEmail(string email);

        void RemoveOrderDetails(int orderDetailsId);
        void PayOrder(OrderDTO orderId);
        void SetShipped(int id);
    }
}
