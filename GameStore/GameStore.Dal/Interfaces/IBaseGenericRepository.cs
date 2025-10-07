namespace GameStore.Dal.Interfaces
{
    public interface IBaseGenericRepository<TEntity>
    {
        void Create(TEntity item);
    }
}