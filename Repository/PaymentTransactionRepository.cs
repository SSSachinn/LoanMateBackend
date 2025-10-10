using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class PaymentTransactionRepository:IPaymentTransactionRepository
    {
        private readonly LoanManagementSystemContext _context;
        public PaymentTransactionRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }
        async Task<PaymentTransaction> IPaymentTransactionRepository.Create(PaymentTransaction transaction)
        {
            _context.PaymentsTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
        async Task<PaymentTransaction> IPaymentTransactionRepository.Delete(int id)
        {
            var existing = await _context.PaymentsTransactions.FirstOrDefaultAsync(t => t.PaymentId == id);
            if (existing != null)
            {
                _context.PaymentsTransactions.Remove(existing);
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        async Task<IEnumerable<PaymentTransaction>> IPaymentTransactionRepository.GetAll()
        {
            return await _context.PaymentsTransactions.ToListAsync();
        }
        async Task<PaymentTransaction> IPaymentTransactionRepository.GetById(int id)
        {
            var existing = await _context.PaymentsTransactions.FirstOrDefaultAsync(t => t.PaymentId == id);
            if (existing == null)
            {
                throw new KeyNotFoundException("Payment Transaction not found");
            }
            return existing;
        }
        public async Task<PaymentTransaction> Update(PaymentTransaction transaction)
        {
            var existing = await _context.PaymentsTransactions
                .FirstOrDefaultAsync(t => t.PaymentId == transaction.PaymentId);

            if (existing != null)
            {
                existing.RepaymentId = transaction.RepaymentId;
                existing.ApplicationId = transaction.ApplicationId;
                existing.Amount = transaction.Amount;
                existing.Gateway = transaction.Gateway;
                //existing.TransactionRef = transaction.TransactionRef; assigned in service layet
                existing.Status = transaction.Status;
                existing.PaidAt = transaction.PaidAt;

                await _context.SaveChangesAsync();
            }

            return existing;
        }
        async Task IPaymentTransactionRepository.SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
