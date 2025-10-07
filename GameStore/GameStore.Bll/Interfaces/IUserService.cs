using System.Collections.Generic;
using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface IUserService
    {
        UserDTO Login(string email, string password);
        void Register(UserDTO userDto);
        void Edit(UserDTO userDto);
        UserDTO GetUser(string email);
        UserDTO GetUserById(int id);
        IEnumerable<UserDTO> GetAllUsers();
        void EditSender(string email, int id);
    }
}
