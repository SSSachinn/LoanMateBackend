using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTransactionController : ControllerBase
    {
        private readonly IPaymentTransactionService _service;
        public PaymentTransactionController(IPaymentTransactionService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult<PaymentTransaction>> Create(PaymentTransaction transaction)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Create(transaction);
                return CreatedAtAction("Get", new { id = transaction.PaymentId }, result);
            }
            return BadRequest(transaction);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentTransaction>>> Get()
        {
            var transactions = await _service.GetAll();
            return Ok(transactions);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentTransaction>> Get(int id)
        {
            var transaction = await _service.GetById(id);
            return Ok(transaction);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentTransaction>> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
        [HttpPost("Confirm/{paymentId}")]
        public async Task<ActionResult<PaymentTransaction>> Confirm(int paymentId, [FromQuery] string transactionRef)
        {
            try
            {
                var result = await _service.Confirm(paymentId, transactionRef);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Fail/{paymentId}")]
        public async Task<ActionResult<PaymentTransaction>> Fail(int paymentId, [FromQuery] string reason = "")
        {
            try
            {
                var result = await _service.Fail(paymentId, reason);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
