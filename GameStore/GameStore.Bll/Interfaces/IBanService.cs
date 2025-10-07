namespace GameStore.Bll.Interfaces
{
    public interface IBanService
    {
        void Ban(int commentId, string period);
        void UnBan(int userId);
    }
}