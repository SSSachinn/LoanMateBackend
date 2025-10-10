using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface ILoanOfficerService
    {
        Task<LoanOfficer> Create(LoanOfficer officer);
        Task<LoanOfficer> GetById(int id);
        Task<IEnumerable<LoanOfficer>> GetAll();
        Task<LoanOfficer> Update(LoanOfficer officer);
        Task<LoanOfficer> Delete(int id);
        Task<LoanOfficer> GetByUserId(int userId);
        Task<LoanOfficer> AssignApplication(int applicationId);
        Task<bool> Activate(int officerId);
        Task<bool> Deactivate(int officerId);
    }
}
