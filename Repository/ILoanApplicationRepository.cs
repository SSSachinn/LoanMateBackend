using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface ILoanApplicationRepository
    {
        Task<LoanApplication> Create(LoanApplication loanApplication);
        Task<LoanApplication> GetById(int id);
        Task<LoanApplication> Update(LoanApplication loanApplication);
        Task<LoanApplication> Delete(int id);
        Task<IEnumerable<LoanApplication>> GetAll();
        Task SaveChangesAsync();
    }
}
