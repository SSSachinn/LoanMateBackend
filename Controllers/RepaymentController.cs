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
    public class RepaymentController : ControllerBase
    {
        private readonly IRepaymentService _service;
        private readonly LoanManagementSystemContext _context;
        private readonly ILoanApplicationService _loanAppService;
        public RepaymentController(IRepaymentService service,LoanManagementSystemContext context, ILoanApplicationService loanAppService)
        {
            _service = service;
            _context = context;
            _loanAppService = loanAppService;

        }


        [HttpPost("Generate/{applicationId}")]
        public async Task<IActionResult> GenerateSchedule(int applicationId)
        {
            var application = await _loanAppService.GetById(applicationId);
            if (application == null) return NotFound();

            await _service.GenerateRepaymentSchedule(application);
            var repayments = await _service.GetAll(); 
            var appRepayments = repayments.Where(r => r.ApplicationId == applicationId)
                                          .OrderBy(r => r.InstallmentNumber);
            return Ok(appRepayments);
        }

        [HttpPost("Pay/{applicationId}")]
        public async Task<IActionResult> PayFirstUnpaid(int applicationId)
        {
            try
            {
                var repayment = await _service.PayInstallment(applicationId);
                return Ok(repayment);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }




        [HttpGet]
        public async Task<ActionResult<IEnumerable<Repayment>>> Get()
        {
            var repayments = await _service.GetAll();
            return Ok(repayments);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Repayment>> Get(int id)
        {
            var repayment = await _service.GetById(id);
            return Ok(repayment);
        }
        [HttpPut]
        public async Task<ActionResult<Repayment>> Put(Repayment repayment)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Update(repayment);
                return Ok(result);
            }
            return BadRequest(repayment);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Repayment>> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }

        

        [HttpGet("by-application/{applicationId}")]
        public async Task<ActionResult<IEnumerable<Repayment>>> GetRepayments(int applicationId)
        {
            var repayments = await _context.Repayments
                .Where(r => r.ApplicationId == applicationId)
                .ToListAsync();

            return Ok(repayments);
        }

        [HttpPost("send-reminder")]
        public async Task<IActionResult> SendReminder()
        {
            await _service.SendRepaymentReminder();
            return Ok("Reminder emails sent.");
        }

    }
}
