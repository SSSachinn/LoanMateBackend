using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IDocumentService
    {
        Task<Document> Create(Document document);
        Task<Document> GetById(int id);
        Task<IEnumerable<Document>> GetAll();
        Task<Document> Update(Document document);
        Task<Document> Delete(int id);
        Task<IEnumerable<Document>> GetDocumentsByApplicationId(int applicationId);
    }
}
