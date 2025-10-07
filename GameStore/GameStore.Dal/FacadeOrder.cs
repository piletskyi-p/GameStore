using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Mongo;
using GameStore.Dal.Interfaces;
using GameStore.Dal.Repositories;

namespace GameStore.Dal
{
    public class FacadeOrder : GenericRepository<Order>, IFacadeOrder
    {
        private readonly IMongoRepository<OrderMongo> _ordeRepositoty;

        public FacadeOrder(
            IDataBaseConnection context, IMongoRepository<OrderMongo> order) : base(context)
        {
            _ordeRepositoty = order;
        }

        public IEnumerable<Order> Get(params Expression<Func<Order, object>>[] includeExpressions)
        {
            var ordersMongo = _ordeRepositoty.Get().ToList();
            var orders = Mapper.Map<List<Order>>(ordersMongo);

            for (int i = 0; i < orders.Count(); ++i)
            {
                orders[i].OrderDate = DateTime.Parse(ordersMongo[i].OrderDateStr);
            }

            var ordersSql = base.Get(includeExpressions).ToList().Union(orders);

            return ordersSql;
        }

        public IEnumerable<Order> Get(Func<Order, bool> predicate,
            params Expression<Func<Order, object>>[] includeExpression)
        {
            var ordersMongo = _ordeRepositoty.Get().ToList();
            var orders = Mapper.Map<List<Order>>(ordersMongo);
            DateTime date;

            for (int i = 0; i < orders.Count(); ++i)
            {
                orders[i].OrderDate = DateTime.Parse(ordersMongo[i].OrderDateStr);
                if (DateTime.TryParse(ordersMongo[i].ShippedDateStr, out date))
                {
                    orders[i].ShippedDate = DateTime.Parse(ordersMongo[i].ShippedDateStr);
                }
            }

            var ordersSql = base.Get(includeExpression).Union(orders);

            return ordersSql.Where(predicate);
        }

        public IEnumerable<Order> GetFromSql(Func<Order, bool> predicate,
            params Expression<Func<Order, object>>[] includeExpression)
        {
            var ordersSql = base.Get(includeExpression);

            return ordersSql.Where(predicate);
        }
    }
}
