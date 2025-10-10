using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface IUserRepository
    {
        Task<User> Create(User user);
        Task<User> GetById(int id);
        Task<User> Update(User user);
        Task<User> Delete(int id);
        Task<IEnumerable<User>> GetAll();
        public Task UpdatePassword(int userId, string newPassword);
    }
}
