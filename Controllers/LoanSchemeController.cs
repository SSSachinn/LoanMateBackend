using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanSchemeController : ControllerBase
    {
        private readonly ILoanSchemeService _service;
        public LoanSchemeController(ILoanSchemeService service)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(Roles =nameof(Role.Admin))]
        public async Task<ActionResult<LoanScheme>> Create(LoanScheme scheme)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Create(scheme);
                return CreatedAtAction("Get", new { id = scheme.SchemeId }, result);
            }
            return BadRequest(scheme);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanScheme>>> Get()
        {
            var schemes = await _service.GetAll();
            return Ok(schemes);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanScheme>> Get(int id)
        {
            var scheme = await _service.GetById(id);
            return Ok(scheme);
        }
        [HttpPut]
        public async Task<ActionResult<LoanScheme>> Put(LoanScheme scheme)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Update(scheme);
                return Ok(result);
            }
            return BadRequest(scheme);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoanScheme>> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
        [HttpPost("activate/{schemeId}")]
        public async Task<IActionResult> Activate(int adminId, int schemeId)
        {
            var result = await _service.Activate(adminId, schemeId);
            if (!result)
                return NotFound("Scheme not found");

            return Ok("Scheme activated successfully.");
        }

        [HttpPost("deactivate/{schemeId}")]
        public async Task<IActionResult> Deactivate(int adminId, int schemeId)
        {
            var result = await _service.Deactivate(adminId, schemeId);
            if (!result)
                return NotFound("Scheme not found");

            return Ok("Scheme deactivated successfully.");
        }
        [HttpGet("Calculate-Interset")]
        public async Task<ActionResult<decimal>> CalculateInterest(int schemeId, decimal amount)
        {
            var interest = await _service.CalculateInterest(schemeId, amount);
            return Ok(interest);
        }
    }
}
