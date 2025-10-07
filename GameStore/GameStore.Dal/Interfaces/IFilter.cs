namespace GameStore.Dal.Interfaces
{
    public interface IFilter<T>
    {
        T Execute(T input);
    }
}