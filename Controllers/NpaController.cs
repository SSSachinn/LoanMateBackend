using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NpaController : ControllerBase
    {
        private readonly INpaService _npaService;

        public NpaController(INpaService npaService)
        {
            _npaService = npaService;
        }

        [HttpPost("Add/{customerId}/{applicationId}")]
        public async Task<ActionResult<Npa>> AddCustomerToNpa(int customerId, int applicationId)
        {
            var npa = await _npaService.AddCustomerToNpaAsync(customerId,applicationId);
            return Ok(npa);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Npa>>> GetAllNpas()
        {
            var npas = await _npaService.GetAllNpasAsync();
            return Ok(npas);
        }

        [HttpGet("TotalOverdue")]
        public async Task<ActionResult<decimal>> GetTotalOverdueAll()
        {
            var total = await _npaService.GetTotalOverdueAllAsync();
            return Ok(total);
        }

        [HttpGet("Filtered")]
        public async Task<ActionResult<IEnumerable<Npa>>> GetFilteredNpas([FromQuery] string sortBy = "date",[FromQuery] bool asc = false)
        {
            var result = await _npaService.GetFilteredNpasAsync(sortBy, asc);
            return Ok(result);
        }


        [HttpGet("Export/Excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] string sortBy = "date", [FromQuery] bool asc = false)
        {
            var fileContent = await _npaService.ExportNpasToExcelAsync(sortBy, asc);
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "NpaReport.xlsx";
            return File(fileContent, contentType, fileName);
        }

        [HttpGet("Export/Pdf")]
        public async Task<IActionResult> ExportToPdf([FromQuery] string sortBy = "date", [FromQuery] bool asc = false)
        {
            var (fileContent, contentType, fileName) = await _npaService.ExportNpasToPdfAsync(sortBy, asc);
            return File(fileContent, contentType, fileName);
        }

    }

}
