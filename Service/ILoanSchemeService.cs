using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface ILoanSchemeService
    {
        Task<LoanScheme> Create(LoanScheme scheme);
        Task<LoanScheme> GetById(int id);
        Task<LoanScheme> Delete(int id);
        Task<IEnumerable<LoanScheme>> GetAll();
        Task<LoanScheme> Update(LoanScheme scheme);
        Task<bool> Activate(int adminId, int schemeId);
        Task<bool> Deactivate(int adminId, int schemeId);
        Task<decimal> CalculateInterest(int schemeId, decimal amount);
    }
}
