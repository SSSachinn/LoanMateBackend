using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface INpaRepository
    {
        Task<decimal> GetCustomerOverdueAmountAsync(int customerId, int applicationId);
        Task AddNpaAsync(Npa npa);
        Task<IEnumerable<Npa>> GetAllNpasAsync();
        Task<decimal> GetTotalOverdueAllAsync();
        public Task SaveChangesAsync();
    }
}
