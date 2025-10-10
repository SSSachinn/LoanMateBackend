using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Service.IUserService _service;
        public UserController(Service.IUserService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Create(user);
                return CreatedAtAction("Get", new { id = user.UserId }, result);
            }
            return BadRequest();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<User>> Get()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(User user)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Update(user);
                return Ok(result);
            }
            return BadRequest(user);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var result = await _service.ChangePassword(new User { UserId = userId }, oldPassword, newPassword);
            if (!result)
            {
                return BadRequest(new { Message = "Old password is incorrect or user not found" });
            }
            return Ok(new { Message = "Password updated successfully" });
        }

    }
}
