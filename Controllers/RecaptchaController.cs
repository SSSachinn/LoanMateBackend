using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecaptchaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RecaptchaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] RecaptchaRequest request)
        {
            var secret = _configuration["Recaptcha:SecretKey"]; // put secret in appsettings.json
            using var client = new HttpClient();
            var response = await client.GetStringAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={request.Token}"
            );

            var result = JsonSerializer.Deserialize<RecaptchaResponse>(response);

            if (result == null || !result.Success)
                return BadRequest(new { success = false });

            return Ok(new { success = true });
        }
    }
}
