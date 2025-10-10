using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface IReportRepository
    {
        Task<Report> Create(Report report); 
        Task<Report> GetById(int id);
        Task<IEnumerable<Report>> GetAll();
        Task<Report> Delete(int id);
        Task<Report> Update(Report report);
        Task SaveChangesAsync();

    }
}
