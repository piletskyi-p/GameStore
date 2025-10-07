using GameStore.Bll.DTO;

namespace GameStore.Bll.Auth
{
    public interface IUserProvider
    {
        UserDTO User { get; set; }
    }
}
