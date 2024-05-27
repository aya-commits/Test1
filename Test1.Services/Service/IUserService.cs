using Domain.Models;

namespace Test1.Services.Service
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        void AddUser(User user);
        User GetUser(int id);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
