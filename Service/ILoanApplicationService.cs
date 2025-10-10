using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface ILoanApplicationService
    {
        Task<LoanApplication> Create(LoanApplication application);
        Task<LoanApplication> GetById(int id);
        Task<IEnumerable<LoanApplication>> GetAll();
        Task<LoanApplication> Update(LoanApplication application);
        Task<LoanApplication> Delete(int id);
        Task<List<DailyLoanCount>> GetDailyLoanCountsAsync(DateTime? start = null, DateTime? end = null);
        Task<LoanApplication> Approve(int applicationId,int officerId);
        Task<LoanApplication> Reject(int applicationId, string reason,int officerId);
        Task<LoanApplication> Close(int applicationId);
        Task<LoanApplication> Disburse(int applicationId);
        Task<LoanApplication> UnderReview(int applicationId);

    }
}
