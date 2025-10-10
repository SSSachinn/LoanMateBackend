using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class ReportRepository:IReportRepository
    {
        private readonly LoanManagementSystemContext _context;
        public ReportRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }
        async Task<Report> IReportRepository.Create(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }
        async Task<Report> IReportRepository.Delete(int id)
        {
            var existing = await _context.Reports.FindAsync(id);
            if (existing != null)
            {
                _context.Reports.Remove(existing);
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        async Task<IEnumerable<Report>> IReportRepository.GetAll()
        {
            return await _context.Reports.ToListAsync();
        }
        async Task<Report> IReportRepository.GetById(int id)
        {
            var existing = await _context.Reports.FindAsync(id);
            if(existing == null)
            {
                throw new KeyNotFoundException("Report not found");
            }   
            return existing;
        }
        public async Task<Report> Update(Report report)
        {
            var existing = await _context.Reports.FindAsync(report.ReportId);
            if (existing != null)
            {
                existing.GeneratedBy = report.GeneratedBy;
                existing.Type = report.Type;
                existing.FilePath = report.FilePath;
                existing.GeneratedAt = report.GeneratedAt;
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
