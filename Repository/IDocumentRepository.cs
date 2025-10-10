using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface IDocumentRepository
    {
        Task<Document> Create(Document document);
        Task<Document> GetById(int id);
        Task<Document> Update(Document document);
        Task<Document> Delete(int id);
        Task<IEnumerable<Document>> GetAll();
        Task<IEnumerable<Document>> GetDocumentsByApplicationId(int applicationId);
    }
}
