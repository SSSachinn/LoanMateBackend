using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IUserService
    {
        Task<User> Create(User user);
        Task<User> GetById(int id);
        Task<IEnumerable<User>> GetAll();
        Task<User> Update(User user);
        Task<User> Delete(int id);
        Task<bool> ChangePassword(User user, string oldPassword, string newPassword);
    }
}
