using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface ILoanOfficerRepository
    {
        Task<LoanOfficer> Create(LoanOfficer loanOfficer);
        Task<LoanOfficer> GetById(int id);
        Task<LoanOfficer> Update(LoanOfficer loanOfficer);
        Task<LoanOfficer> Delete(int id);
        Task<IEnumerable<LoanOfficer>> GetAll();
        Task<LoanOfficer> GetByUserId(int userId);
        public Task<List<LoanOfficer>> GetByCityAsync(string city);
        public Task<List<LoanOfficer>> GetActiveOfficersAsync();
        public Task SaveChangesAsync();
    }
}
