using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repos;
        public UserService(Repository.IUserRepository repos)
        {
            _repos = repos;
        }
        async Task<User> IUserService.Create(User user)
        {
            return await _repos.Create(user);
        }
        async Task<User> IUserService.Delete(int id)
        {
            return await _repos.Delete(id);
        }
        async Task<IEnumerable<User>> IUserService.GetAll()
        {
            return await _repos.GetAll();
        }
        async Task<User> IUserService.GetById(int id)
        {
            return await _repos.GetById(id);
        }
        async Task<User> IUserService.Update(User user)
        {
            return await _repos.Update(user);
        }
        async Task<bool> IUserService.ChangePassword(User user, string oldPassword, string newPassword)
        {
            var existing = await _repos.GetById(user.UserId);
            if (existing == null || existing.Password != oldPassword)
                return false;

            await _repos.UpdatePassword(user.UserId, newPassword);
            return true;
        }
    }
}
