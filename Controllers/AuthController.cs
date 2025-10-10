using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {

            LoginResponse response;
            if (ModelState.IsValid)
            {
                response = await _authService.Login(loginRequest);
                var token = response.Token;
                if (response.IsSuccess && response.User != null)
                {
                    return Ok(new { token });
                }
                return Unauthorized(new { Message = "Invalid email or password" });
            }
            return BadRequest();
        }


        [HttpPost("SendOtp")]
        public async Task<IActionResult> SendOtp([FromBody] string email)
        {
            bool result = await _authService.SendOtpAsync(email);
            if (result)
                return Ok(new { message = "OTP sent successfully" });
            return BadRequest(new { message = "Failed to send OTP" });
        }

        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            bool isValid = await _authService.VerifyOtpAsync(request.Email, request.Otp);
            if (isValid)
                return Ok(new { valid = true });
            return BadRequest(new { valid = false, message = "Invalid OTP" });
        }
    }
}


