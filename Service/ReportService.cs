using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class ReportService: IReportService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<Report> Create(Report report)
        {
            return await _reportRepository.Create(report);
        }
        public async Task<Report> Delete(int id)
        {
            return await _reportRepository.Delete(id);
        }
        public async Task<IEnumerable<Report>> GetAll()
        {
            return await _reportRepository.GetAll();
        }
        public async Task<Report> GetById(int id)
        {
            return await _reportRepository.GetById(id);
        }
        public async Task<Report> Update(Report report)
        {
            return await _reportRepository.Update(report);
        }

    }
}
