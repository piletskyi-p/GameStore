using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GameStore.Dal.Entities;

namespace GameStore.Dal.Interfaces
{
    public interface IFacadeOrder : IGenericRepository<Order>
    {
        IEnumerable<Order> Get(params Expression<Func<Order, object>>[] includeExpression);
        IEnumerable<Order> Get(Func<Order, bool> predicate,
            params Expression<Func<Order, object>>[] includeExpression);
        IEnumerable<Order> GetFromSql(Func<Order, bool> predicate,
            params Expression<Func<Order, object>>[] includeExpression);
    }
}