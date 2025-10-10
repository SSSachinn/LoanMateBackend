using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface INpaService
    {
        Task<Npa> AddCustomerToNpaAsync(int customerId, int applicationId);
        Task<IEnumerable<Npa>> GetAllNpasAsync();
        Task<decimal> GetTotalOverdueAllAsync();
        Task<IEnumerable<Npa>> GetFilteredNpasAsync(string sortBy, bool ascending);

        Task<byte[]> ExportNpasToExcelAsync(string sortBy, bool asc);
        Task<(byte[] FileContent, string ContentType, string FileName)> ExportNpasToPdfAsync(string sortBy, bool asc);
    }
}
