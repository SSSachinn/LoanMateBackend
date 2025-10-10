using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class DocumentRepository: IDocumentRepository
    {
        private readonly LoanManagementSystemContext _context;
        public DocumentRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }
        async Task<Document> IDocumentRepository.Create(Document document)
        {
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }
        async Task<Document> IDocumentRepository.Delete(int id)
        {
            var existing = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == id);
            if (existing != null)
            {
                _context.Documents.Remove(existing);
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        async Task<IEnumerable<Document>> IDocumentRepository.GetAll()
        {
            return await _context.Documents.ToListAsync();
        }
        async Task<Document> IDocumentRepository.GetById(int id)
        {
            var existing = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == id);
            if (existing == null)
            {
                throw new KeyNotFoundException("Document not found");
            }   
            return existing;
        }
        async Task<Document> IDocumentRepository.Update(Document document)
        {
            var existing = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == document.DocumentId);
            if (existing != null)
            {
                existing.ApplicationId = document.ApplicationId;
                existing.DocType = document.DocType;
                existing.FilePath = document.FilePath;
                existing.UploadedAt = document.UploadedAt;
                existing.VerifiedBy = document.VerifiedBy;
                existing.VerificationStatus = document.VerificationStatus;
                await _context.SaveChangesAsync();
            }

            return existing;
        }

        async Task<IEnumerable<Document>> IDocumentRepository.GetDocumentsByApplicationId(int applicationId)
        {
            return await _context.Documents
                        .Where(d => d.ApplicationId == applicationId)
                        .ToListAsync();
        }

    }
}

