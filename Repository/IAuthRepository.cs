using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface IAuthRepository
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        
    }
}
