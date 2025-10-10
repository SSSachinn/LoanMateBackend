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
    public class LoanApplicationController : ControllerBase
    {
        private readonly ILoanApplicationService _service;
        private readonly LoanManagementSystemContext _context;
        public LoanApplicationController(ILoanApplicationService service,LoanManagementSystemContext context)
        {
            _service = service;
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<LoanApplication>> Create(LoanApplication application)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Create(application);
                return CreatedAtAction("Get", new { id = application.ApplicationId }, result);
            }
            return BadRequest(application);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanApplication>>> Get()
        {
            var applications = await _service.GetAll();
            return Ok(applications);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanApplication>> Get(int id)
        {
            var application = await _service.GetById(id);
            return Ok(application);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<LoanApplication>> Put(int id,LoanApplication application)
        {
            if(id!= application.ApplicationId)
            {
                return BadRequest("Application ID mismatch");
            }
            if (ModelState.IsValid)
            {
                var result = await _service.Update(application);
                return Ok(result);
            }
            return BadRequest(application);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoanApplication>> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
        [HttpPut("UnderReview/{applicationId}")]
        public async Task<ActionResult<LoanApplication>> UnderReview(int applicationId)
        {
            var result = await _service.UnderReview(applicationId);
            return Ok(result);
        }

        [HttpGet("daily-loans")]
        public async Task<ActionResult<List<DailyLoanCount>>> GetDailyLoans()
        {
            var result = await _service.GetDailyLoanCountsAsync();
            return Ok(result);
        }


        [HttpPut("Approve/{applicationId}")]
        public async Task<ActionResult<LoanApplication>> Approve(int applicationId, int officerId)
        {
            var result = await _service.Approve(applicationId,officerId);
            return Ok(result);
        }
        [HttpPut("Reject/{applicationId}")]
        public async Task<ActionResult<LoanApplication>> Reject(int applicationId, [FromBody] string reason, int officerId)
        {
            var result = await _service.Reject(applicationId, reason,officerId);
            return Ok(result);
        }
        [HttpPut("Disburse/{applicationId}")]
        public async Task<ActionResult<LoanApplication>> Disburse(int applicationId)
        {
            var result = await _service.Disburse(applicationId);
            return Ok(result);
        }
        [HttpPut("Close/{applicationId}")]
        public async Task<ActionResult<LoanApplication>> Close(int applicationId)
        {
            var result = await _service.Close(applicationId);
            return Ok(result);
        }

        [HttpGet("applications/{applicationId}/repayments/exists")]
        public async Task<ActionResult<bool>> HasRepayments(int applicationId)
        {
            bool hasRepayments = await _context.Repayments
                .AnyAsync(r => r.ApplicationId == applicationId);

            return Ok(hasRepayments);
        }
    }
}
