using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Service
{
    public class DocumentService: IDocumentService
    {
        private readonly IDocumentRepository _repos;
        public DocumentService(IDocumentRepository repos)
        {
            _repos = repos;
        }
        async Task<Document> IDocumentService.Create(Document document)
        {
            return await _repos.Create(document);
        }
        async Task<Document> IDocumentService.Delete(int id)
        {
            return await _repos.Delete(id);
        }
        async Task<IEnumerable<Document>> IDocumentService.GetAll()
        {
            return await _repos.GetAll();
        }
        async Task<Document> IDocumentService.GetById(int id)
        {
            return await _repos.GetById(id);
        }
        async Task<Document> IDocumentService.Update(Document document)
        {
            return await _repos.Update(document);
        }

        async Task<IEnumerable<Document>> IDocumentService.GetDocumentsByApplicationId(int applicationId)
        {
            return await _repos.GetDocumentsByApplicationId(applicationId);
        }


    }
}
