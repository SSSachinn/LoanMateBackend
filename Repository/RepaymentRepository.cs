using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class RepaymentRepository: IRepaymentRepository
    {
        private readonly LoanManagementSystemContext _context;
        public RepaymentRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }
        async Task<Repayment> IRepaymentRepository.Create(Repayment repayment)
        {
            _context.Repayments.Add(repayment);
            await _context.SaveChangesAsync();
            return repayment;
        }
        async Task<Repayment> IRepaymentRepository.Delete(int id)
        {
            var existing = await _context.Repayments.FirstOrDefaultAsync(r => r.RepaymentId == id);
            if (existing != null)
            {
                _context.Repayments.Remove(existing);
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        async Task<IEnumerable<Repayment>> IRepaymentRepository.GetAll()
        {
            return await _context.Repayments.ToListAsync();
        }
        async Task<Repayment> IRepaymentRepository.GetById(int id)
        {
            var repayment = await _context.Repayments.FirstOrDefaultAsync(r => r.RepaymentId == id);

            if (repayment == null)
            {
                // Debug log
                throw new KeyNotFoundException($"Repayment with Id {id} not found in DB");
            }

            return repayment;
        }
        public async Task<Repayment> Update(Repayment repayment)
        {
            var existing = await _context.Repayments.FirstOrDefaultAsync(r => r.RepaymentId == repayment.RepaymentId);
            if (existing != null)
            {
                // Update all relevant repayment fields
                existing.ApplicationId = repayment.ApplicationId;
                existing.InstallmentNumber = repayment.InstallmentNumber;
                existing.AmountEMI = repayment.AmountEMI;
                existing.AmountPaid = repayment.AmountPaid;
                existing.AmountDue = repayment.AmountDue;
                existing.DueDate = repayment.DueDate;
                existing.IsOverdue = repayment.IsOverdue;
                existing.PenaltyAmount = repayment.PenaltyAmount;
                existing.Status = repayment.Status;

                await _context.SaveChangesAsync();
            }
            return existing;
        }

        async Task IRepaymentRepository.SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<decimal> GetTotalPaidByApplication(int applicationId)
        {
            return await _context.Repayments
                .Where(r => r.ApplicationId == applicationId)
                .SumAsync(r => r.AmountPaid);
        }
        public async Task DeleteAllByApplicationId(int applicationId)
        {
            var repayments = await _context.Repayments.Where(r => r.ApplicationId == applicationId && r.AmountDue > 0).ToListAsync();

            if (repayments.Any())
            {
                _context.Repayments.RemoveRange(repayments);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Repayment>> GetAllByApplicationId(int applicationId)
        {
            return await _context.Repayments
                                 .Where(r => r.ApplicationId == applicationId)
                                 .OrderBy(r => r.InstallmentNumber)
                                 .ToListAsync();
        }
        public async Task BulkInsert(List<Repayment> repayments)
        {
            await _context.Repayments.AddRangeAsync(repayments);
            await _context.SaveChangesAsync();
        }

    }
}
