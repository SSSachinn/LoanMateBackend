using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface IRepaymentRepository
    {
        Task<Repayment> Create(Repayment repayment);
        Task<Repayment> GetById(int id);
        Task<Repayment> Update(Repayment repayment);
        Task<Repayment> Delete(int id);
        Task<IEnumerable<Repayment>> GetAll();
        Task SaveChangesAsync();
        Task<decimal> GetTotalPaidByApplication(int applicationId);
        Task DeleteAllByApplicationId(int applicationId);
        Task<IEnumerable<Repayment>> GetAllByApplicationId(int applicationId);
        public Task BulkInsert(List<Repayment> repayments);
    }
}
