using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class LoanOfficerRepository: ILoanOfficerRepository
    {
        private readonly LoanManagementSystemContext _context;
        public LoanOfficerRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }
        async Task<LoanOfficer> ILoanOfficerRepository.Create(LoanOfficer loanOfficer)
        {
            _context.LoanOfficers.Add(loanOfficer);
            await _context.SaveChangesAsync();
            return loanOfficer;
        }
        async Task<LoanOfficer> ILoanOfficerRepository.Delete(int id)
        {
            var existing = await _context.LoanOfficers.FirstOrDefaultAsync(o => o.OfficerId == id);
            if (existing != null)
            {
                _context.LoanOfficers.Remove(existing);
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        async Task<IEnumerable<LoanOfficer>> ILoanOfficerRepository.GetAll()
        {
            return await _context.LoanOfficers.Include(u=>u.User).ToListAsync();
        }
        async Task<LoanOfficer> ILoanOfficerRepository.GetById(int id)
        {
            var existing = await _context.LoanOfficers.Include(u=>u.User).FirstOrDefaultAsync(o => o.OfficerId == id);
            if (existing == null)
            {
                throw new KeyNotFoundException("Loan Officer not found");
            }   
            return existing;
        }
        async Task<LoanOfficer> ILoanOfficerRepository.Update(LoanOfficer loanOfficer)
        {
            var existing = await _context.LoanOfficers.FirstOrDefaultAsync(o => o.OfficerId == loanOfficer.OfficerId);
            if (existing != null)
            {
                existing.UserId = loanOfficer.UserId;
                existing.FullName = loanOfficer.FullName;
                existing.City = loanOfficer.City;
                existing.Active = loanOfficer.Active;
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        public async Task<LoanOfficer> GetByUserId(int userId)
        {
            return await _context.LoanOfficers.FirstOrDefaultAsync(o => o.UserId == userId);
        }
        public async Task<List<LoanOfficer>> GetByCityAsync(string city)
        {
            return await _context.LoanOfficers.Where(c => c.City == city && c.Active).ToListAsync();
        }
        public async Task<List<LoanOfficer>> GetActiveOfficersAsync()
        {
            return await _context.LoanOfficers.Where(o => o.Active).ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
