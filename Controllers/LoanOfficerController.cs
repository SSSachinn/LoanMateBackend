using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanOfficerController : ControllerBase
    {
        private readonly ILoanOfficerService _service;
        private readonly LoanManagementSystemContext _context;
        public LoanOfficerController(ILoanOfficerService service, LoanManagementSystemContext context)
        {
            _service = service;
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<LoanOfficer>> Create(LoanOfficer officer)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Create(officer);
                return CreatedAtAction("Get", new { id = officer.OfficerId }, result);
            }
            return BadRequest(officer);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanOfficer>>> Get()
        {
            var officers = await _service.GetAll();
            return Ok(officers);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanOfficer>> Get(int id)
        {
            var officer = await _service.GetById(id);
            return Ok(officer);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<LoanOfficer>> Put(LoanOfficer officer)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Update(officer);
                return Ok(result);
            }
            return BadRequest(officer);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoanOfficer>> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<Customer>> GetOfficerByUserId(int userId)
        {

            var officer = await _context.LoanOfficers
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (officer == null)
                return NotFound("No Officer found for this userId.");

            return Ok(officer.OfficerId);
        }

        [HttpPost("Assign-LoanOfficer")]
        public async Task<ActionResult<LoanOfficer>> AssignLoanOfficer(int applicationId)
        {
            var officer = await _service.AssignApplication(applicationId);
            if (officer == null)
            {
                return NotFound("No loan officer available");
            }
            return Ok(officer);
        }
        [HttpPost("Activate")]
        public async Task<IActionResult> Activate(int officerId)
        {
            var result = await _service.Activate(officerId);
            if (!result)
            {
                return NotFound("Officer not found");
            }
            return Ok(result);
        }
        [HttpPost("Deactivate")]
        public async Task<IActionResult> Deactivate(int officerId)
        {
            var result = await _service.Deactivate(officerId);
            if (!result)
            {
                return NotFound("Officer not found");
            }
            return Ok(result);
        }

    }
}
