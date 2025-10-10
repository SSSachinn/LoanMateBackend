using Castle.Core.Resource;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoanManagementSystem.Service
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerService _customerService;
        private readonly ILoanOfficerService _officerService;
        private readonly IEmailService _emailService;
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        private static readonly Dictionary<string, string> OtpStore = new();
        public AuthService(IAuthRepository authRepository, IConfiguration config, ICustomerService customerService, ILoanOfficerService officerService, IEmailService emailService)
        {
            _authRepository = authRepository;
            _config = config;
            _customerService = customerService;
            _officerService = officerService;
            _emailService = emailService;
        }


        public async Task<bool> SendOtpAsync(string email)
        {
            string otp = new Random().Next(100000, 999999).ToString();
            OtpStore[email] = otp;

            string subject = "Your OTP for LoanMate Signup";
            string body = $"Your OTP is {otp}. It is valid for 5 minutes.";
            await _emailService.SendEmailAsync(email, subject, body);

            // Optional: Implement a timer to auto-expire OTP after 5 mins
            _ = Task.Delay(TimeSpan.FromMinutes(5)).ContinueWith(_ => OtpStore.Remove(email));

            return true;
        }

        public Task<bool> VerifyOtpAsync(string email, string otp)
        {
            if (OtpStore.TryGetValue(email, out string correctOtp) && correctOtp == otp)
            {
                OtpStore.Remove(email); // remove once verified
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }


        async Task<LoginResponse> IAuthService.Login(LoginRequest model)
        {
            var response = await _authRepository.Login(model);
            if (response.IsSuccess)
            {
                response.Token = GenerateToken(response.User);
            }
            return response;
        }

        private string GenerateToken(User user)
        {
            var Secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));
            var SigningCredentials = new SigningCredentials(Secretkey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
            new Claim(ClaimTypes.MobilePhone,user.Phone),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Role,user.Role.ToString()),
            new Claim("Username",user.Username)


        };
            var tokenOptions = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["JwtSettings:ExpiryInMinutes"])),
                signingCredentials: SigningCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

    }

}
