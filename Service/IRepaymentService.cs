using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IRepaymentService
    {
        Task<Repayment> PayInstallment(int applicationId);
        Task<Repayment> GetById(int id);
        Task<IEnumerable<Repayment>> GetAll();
        Task<Repayment> Update(Repayment repayment);
        Task<Repayment> Delete(int id);
        Task GenerateRepaymentSchedule(LoanApplication loanApplication, int remainingMonths = 0, int paidInstallments = 0);

        Task MarkOverdueRepaymentsAsync();
        Task SendRepaymentReminder();
        

    }
}
