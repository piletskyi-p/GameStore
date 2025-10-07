using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;

namespace GameStore.Dal.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> 
        where TEntity : BaseEntity
    {
        private readonly IDataBaseConnection _context;
        private readonly DbSet<TEntity> _setDb;

        public GenericRepository(IDataBaseConnection context)
        {
            _context = context;
            _setDb = context.Set<TEntity>();
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public void Create(TEntity item)
        {
            _setDb.Add(item);
        }

        public TEntity FindById(int? id,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var set = _setDb.Where(entity => !entity.IsDeleted && entity.Id == id);

            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }

            return set.FirstOrDefault();
        }

        //public TEntity FindById(int? id)
        //{
        //    return _setDb.Where(entity => !entity.IsDeleted).ToList().Find(entity => entity.Id == id);
        //}

        //public virtual IEnumerable<TEntity> Get()
        //{
        //    return _setDb.Where(entity => !entity.IsDeleted).ToList();
        //}

        public IEnumerable<TEntity> Get(
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var set = _setDb.Where(entity => !entity.IsDeleted);

            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }

            return set.ToList();
        }

        //public virtual IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        //{
        //    return _setDb.Where(predicate).Where(entity => !entity.IsDeleted).ToList();
        //}

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, 
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var set = _setDb.Where(predicate).Where(entity => !entity.IsDeleted);

            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }

            return set.ToList();
        }

        public void HardDelete(TEntity item)
        {
            _setDb.Remove(item);
        }

        public void Delete(int id)
        {
            var entity = _setDb.FirstOrDefault(del => del.Id == id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _setDb.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public IEnumerable<TEntity> GetByRange(int page, int pageSize, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var set = _setDb.Where(entity => !entity.IsDeleted).OrderBy(game => game.Id)
                .Skip((page - 1) * pageSize).Take(pageSize);

            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }

            return set.ToList();
        }

        public IEnumerable<TEntity> GetDeleted(params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var set = _setDb.Where(entity => entity.IsDeleted);

            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }

            return set.ToList();
        }

        public IEnumerable<TEntity> GetDeleted(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var set = _setDb.Where(predicate).Where(entity => entity.IsDeleted);

            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }

            return set.ToList();
        }

        public IEnumerable<TEntity> GetFromAll(params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var set = _setDb.Where(entity => !entity.IsDeleted && entity.IsDeleted);

            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }

            return set.ToList();
        }

        public IEnumerable<TEntity> GetFromAll(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var set = _setDb.Where(predicate);

            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }

            return set.ToList();
        }
    }
}