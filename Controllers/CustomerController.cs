using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Data;
namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly LoanManagementSystemContext _context;
        public CustomerController(ICustomerService service,LoanManagementSystemContext context)
        {
            _service = service;
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<Customer>> Post(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var newCustomer = await _service.Create(customer);
                return CreatedAtAction("Get", new { id = newCustomer.Customer_Id }, newCustomer);
            }
            return BadRequest(customer);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> Get()
        {
            var authors = await _service.GetAll();
            return Ok(authors);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
        {
            var authors = await _service.GetById(id);
            if (authors == null)
            {
                return BadRequest(authors);
            }
            return Ok(authors);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> Put(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var updated = await _service.Update(customer);
                return Ok(updated);
            }
            return BadRequest(customer);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            var deleted = await _service.Delete(id);
            return Ok(deleted);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<Customer>> GetCustomerByUserId(int userId)
        {
            
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerUserId == userId);

            if (customer == null)
                return NotFound("No customer found for this userId.");

            return Ok(customer.Customer_Id); 
        }



        [HttpPut("UpdateProfile")]
        public async Task<ActionResult> UpdateProfile(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var success = await _service.UpdateProfile(customer);
                if (success)
                    return NoContent();
                else
                    return NotFound();
            }
            return BadRequest(customer);
        }
        [HttpPost("verify-kyc")]
        public async Task<ActionResult> VerifyKYC(int adminId, int customerId, string aadhaarId)
        {
            if (string.IsNullOrEmpty(aadhaarId))
                return BadRequest("AadhaarId is required.");

            var customer = await _service.VerifyKYC(adminId, customerId, aadhaarId);

            if (customer == null)
                return NotFound("Customer not found.");

            return Ok(new
            {
                CustomerId = customer.Customer_Id,
                CustomerName = customer.Customer_Name,
                KYCStatus = customer.KYCStatus.ToString(),
                VerifiedByAdmin = adminId
            });
        }

    }
}
