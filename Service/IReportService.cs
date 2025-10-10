using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IReportService
    {
        Task<Report> Create(Report report);
        Task<Report> GetById(int id);
        Task<IEnumerable<Report>> GetAll();
        Task<Report> Delete(int id);
        Task<Report> Update(Report report);
    }
}
