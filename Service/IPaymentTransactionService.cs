using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IPaymentTransactionService
    {
        Task<PaymentTransaction> Create(PaymentTransaction transaction);
        Task<PaymentTransaction> GetById(int id);
        Task<IEnumerable<PaymentTransaction>> GetAll();
        Task<PaymentTransaction> Update(PaymentTransaction transaction);
        Task<PaymentTransaction> Delete(int id);
        Task<PaymentTransaction> Confirm(int paymentId, string transactionRef);
        Task<PaymentTransaction> Fail(int paymentId, string reason = "");

    }
}
