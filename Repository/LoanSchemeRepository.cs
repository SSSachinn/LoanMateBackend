using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class LoanSchemeRepository: ILoanSchemeRepository
    {
        private readonly LoanManagementSystemContext _context;
        public LoanSchemeRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }
        async Task<LoanScheme> ILoanSchemeRepository.Create(LoanScheme loanScheme)
        {
            _context.LoanSchemes.Add(loanScheme);
            await _context.SaveChangesAsync();
            return loanScheme;
        }
        async Task<LoanScheme> ILoanSchemeRepository.Delete(int id)
        {
            var existing = await _context.LoanSchemes.FirstOrDefaultAsync(s => s.SchemeId == id);
            if (existing != null)
            {
                _context.LoanSchemes.Remove(existing);
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        async Task<IEnumerable<LoanScheme>> ILoanSchemeRepository.GetAll()
        {
            return await _context.LoanSchemes.ToListAsync();
        }
        async Task<LoanScheme?> ILoanSchemeRepository.GetById(int id)
        {
            var existing = await _context.LoanSchemes.FirstOrDefaultAsync(s => s.SchemeId == id);
            if (existing == null)
            {
                throw new KeyNotFoundException("Loan Scheme not found");
            }   
            return existing;
        }
        async Task<LoanScheme> ILoanSchemeRepository.Update(LoanScheme loanScheme)
        {
            var existing = await _context.LoanSchemes.FirstOrDefaultAsync(s => s.SchemeId == loanScheme.SchemeId);
            if (existing != null)
            {
                existing.Name = loanScheme.Name;
                existing.MinAmount = loanScheme.MinAmount;
                existing.MaxAmount = loanScheme.MaxAmount;
                existing.InterestRate = loanScheme.InterestRate;
                existing.TermMonths = loanScheme.TermMonths;
                existing.Description = loanScheme.Description;
                existing.Active = loanScheme.Active;
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        async Task<LoanScheme> ILoanSchemeRepository.SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
            return null;

        }
    }
}
