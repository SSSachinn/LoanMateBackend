using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LoanManagementSystem.Repository
{
    public class NpaRepository : INpaRepository
    {
        private readonly LoanManagementSystemContext _context;

        public NpaRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetCustomerOverdueAmountAsync(int customerId, int applicationId)
        {
            //Test

            return await _context.Repayments
             .Where(r => r.LoanApplication.CustomerId == customerId &&
                r.Status == RepaymentStatus.Pending && r.ApplicationId==applicationId)
              .SumAsync(r => r.AmountDue + r.PenaltyAmount);


            //return await _context.Repayments
            //    .Where(r =>
            //        r.LoanApplication.CustomerId == customerId &&
            //        (r.Status == RepaymentStatus.Overdue || r.IsOverdue) && r.ApplicationId == applicationId)
            //    .SumAsync(r => r.AmountDue + r.PenaltyAmount);
        }

        public async Task AddNpaAsync(Npa npa)
        {
            await _context.Npas.AddAsync(npa);
        }

        public async Task<IEnumerable<Npa>> GetAllNpasAsync()
        {
            return await _context.Npas
                .Include(n => n.Customer)
                .ThenInclude(c => c.User)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalOverdueAllAsync()
        {
            return await _context.Npas.SumAsync(n => n.TotalOverdue);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
