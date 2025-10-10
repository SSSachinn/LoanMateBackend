using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;
        public DocumentController(IDocumentService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult<Document>> Post(Document document)
        {
            if (ModelState.IsValid)
            {
                var newDocument = await _service.Create(document);
                return CreatedAtAction("Get", new { id = newDocument.DocumentId }, newDocument);
            }
            return BadRequest(document);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> Get()
        {
            var documents = await _service.GetAll();
            return Ok(documents);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> Get(int id)
        {
            var document = await _service.GetById(id);
            return Ok(document);
        }
        [HttpPut]
        public async Task<ActionResult<Document>> Put(Document document)
        {
            if (ModelState.IsValid)
            {
                var updated = await _service.Update(document);
                return Ok(updated);
            }
            return BadRequest(document);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Document>> Delete(int id)
        {
            var deleted = await _service.Delete(id);
            return Ok(deleted);
        }

        [HttpGet("by-application/{applicationId}")]
        public async Task<ActionResult<IEnumerable<Document>>> GetByApplication(int applicationId)
        {
            var docs = await _service.GetDocumentsByApplicationId(applicationId);
            return Ok(docs);
        }

    }
}
