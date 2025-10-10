using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly LoanManagementSystemContext _context;
       public UserRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }
        async Task<User> IUserRepository.Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        async Task<User> IUserRepository.GetById(int id)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u=>u.UserId==id);
            if(existing == null)
            {
                throw new KeyNotFoundException("User not found");
            }   
            return existing;
        }
        async Task<User> IUserRepository.Update(User user)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if (existing != null)
            {
                existing.Username = user.Username;
                existing.Password = user.Password;
                existing.Email = user.Email;
                existing.Phone = user.Phone;
                existing.Role = user.Role;
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        async Task<User> IUserRepository.Delete(int id)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u=>u.UserId==id);
            if (existing != null)
            {
                _context.Users.Remove(existing);
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        async Task<IEnumerable<User>> IUserRepository.GetAll()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task UpdatePassword(int userId, string newPassword)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (existing != null)
            {
                existing.Password = newPassword;
                await _context.SaveChangesAsync();
            }
        }
    }
}
