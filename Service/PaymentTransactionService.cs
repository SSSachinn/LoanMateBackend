using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using System.Security.Cryptography;

namespace LoanManagementSystem.Service
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IPaymentTransactionRepository _repository;
        public PaymentTransactionService(IPaymentTransactionRepository repository)
        {
            _repository = repository;
        }
        async Task<PaymentTransaction> IPaymentTransactionService.Create(PaymentTransaction payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Payment cannot be null");

            payment.Status = PaymentStatus.Pending;
            payment.PaidAt = DateTime.UtcNow;


            payment.TransactionRef = GenerateTransactionRef(payment.Gateway);


            var createdPayment = await _repository.Create(payment);
            await _repository.SaveChangesAsync();

            return createdPayment;
        }
        public async Task<PaymentTransaction> Confirm(int paymentId, string transactionRef)
        {
            var payment = await _repository.GetById(paymentId);
            if (payment == null || payment.Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment not found or already processed.");

            payment.Status = PaymentStatus.Approved;
            payment.TransactionRef = transactionRef;
            payment.PaidAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();
            return payment;
        }
        public async Task<PaymentTransaction> Fail(int paymentId, string reason = "")
        {
            var payment = await _repository.GetById(paymentId);
            if (payment == null || payment.Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Payment not found or already processed.");

            payment.Status = PaymentStatus.Failed;
            if (!string.IsNullOrEmpty(reason))
            {
                payment.TransactionRef = $"{payment.TransactionRef} - {reason}";
            }
            payment.PaidAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();
            return payment;
        }
        async Task<PaymentTransaction> IPaymentTransactionService.Delete(int id)
        {
            return await _repository.Delete(id);
        }
        async Task<IEnumerable<PaymentTransaction>> IPaymentTransactionService.GetAll()
        {
            return await _repository.GetAll();
        }
        async Task<PaymentTransaction> IPaymentTransactionService.GetById(int id)
        {
            return await _repository.GetById(id);
        }
        async Task<PaymentTransaction> IPaymentTransactionService.Update(PaymentTransaction transaction)
        {
            return await _repository.Update(transaction);
        }
        private string GenerateTransactionRef(PaymentGateway gateway)
        {
            if (gateway == PaymentGateway.UPI || gateway == PaymentGateway.NetBanking)
            {
                int length = gateway == PaymentGateway.UPI ? 12 : 16;
                var bytes = new byte[length];
                RandomNumberGenerator.Fill(bytes);
                return string.Concat(bytes.Select(b => (b % 10).ToString())).Substring(0, length);
            }
            else if (gateway == PaymentGateway.Card)
            {
                return $"**** **** **** {new Random().Next(1000, 9999)}";
            }
            else if (gateway == PaymentGateway.Wallet)
            {
                return $"WALLET-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            }

            return Guid.NewGuid().ToString("N");
        }
    }

}
