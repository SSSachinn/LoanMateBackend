using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly LoanManagementSystemContext _context;
        public AuthRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }

        async Task<LoginResponse> IAuthRepository.Login(LoginRequest login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.UserName && u.Password == login.Password);

            LoginResponse response;

            if (user != null)
            {
                response = new LoginResponse { IsSuccess = true, User = user, Token = "" };
                return response;
            }

            response = new LoginResponse { IsSuccess = false, User = null, Token = "" };
            return response;
        }


    }


}

