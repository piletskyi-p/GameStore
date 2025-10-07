using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GameStore.Dal.Entities;

namespace GameStore.Dal.Interfaces
{
    public interface IGenericRepository<TEntity> : IBaseGenericRepository<TEntity>
        where TEntity : BaseEntity
    {
        TEntity FindById(int? id, params Expression<Func<TEntity, object>>[] includeExpressions);

        IEnumerable<TEntity> GetByRange(int page, int pageSize, params Expression<Func<TEntity, object>>[] includeExpressions);

        IEnumerable<TEntity> Get(params Expression<Func<TEntity, object>>[] includeExpressions);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeExpressions);

        IEnumerable<TEntity> GetDeleted(params Expression<Func<TEntity, object>>[] includeExpressions);
        IEnumerable<TEntity> GetDeleted(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeExpressions);

        IEnumerable<TEntity> GetFromAll(params Expression<Func<TEntity, object>>[] includeExpressions);
        IEnumerable<TEntity> GetFromAll(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeExpressions);

        void HardDelete(TEntity item);
        void Update(TEntity item);
        void Delete(int id);
    }
}
