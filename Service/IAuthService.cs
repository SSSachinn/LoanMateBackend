using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IAuthService
    {
        Task<bool> SendOtpAsync(string email);
        Task<bool> VerifyOtpAsync(string email, string otp);
        Task<LoginResponse> Login(LoginRequest loginRequest);

    }
}
