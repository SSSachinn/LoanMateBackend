using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _service;
        public ReportController(IReportService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult<Report>> Create(Report report)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Create(report);

                return CreatedAtAction(nameof(Get), new { id = report.ReportId }, result);
            }
            return BadRequest(report);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> Get(int id)
        {
            var result = await _service.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> Get()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }


        [HttpPut]
        public async Task<ActionResult<Report>> Put(Report report)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Update(report);
                return Ok(result);
            }
            return BadRequest(report);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Report>> Delete(int id)
        {
            var result = await _service.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
