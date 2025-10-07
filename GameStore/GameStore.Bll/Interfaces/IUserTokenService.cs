using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface IUserTokenService
    {
        void Create(UserTokenDto userToken);
        UserTokenDto GetById(int id);
        UserTokenDto GetByToken(string token);
        UserTokenDto GetByUserId(int userId);
        int GetUserIdByToken(string token);
    }
}