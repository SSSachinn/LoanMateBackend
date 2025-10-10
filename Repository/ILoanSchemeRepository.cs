using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface ILoanSchemeRepository
    {
        Task<LoanScheme> Create(LoanScheme loanScheme);
        Task<LoanScheme> GetById(int id);
        Task<LoanScheme> Update(LoanScheme loanScheme);
        Task<LoanScheme> Delete(int id);
        Task<IEnumerable<LoanScheme>> GetAll();
        Task<LoanScheme> SaveChangesAsync();
    }
}
