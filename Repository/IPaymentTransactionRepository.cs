using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface IPaymentTransactionRepository
    {
        Task<PaymentTransaction> Create(PaymentTransaction transaction);
        Task<PaymentTransaction> GetById(int id);
        Task<IEnumerable<PaymentTransaction>> GetAll();
        Task<PaymentTransaction> Update(PaymentTransaction transaction);
        Task<PaymentTransaction> Delete(int id);
        Task SaveChangesAsync();
    }
}
