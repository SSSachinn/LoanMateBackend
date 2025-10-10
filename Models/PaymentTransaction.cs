using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
    public enum PaymentGateway
    {
        UPI,
        NetBanking,
        Card,
        Wallet
    }

    public enum PaymentStatus
    {
        Pending,
        Approved,
        Rejected,
        Completed,
        Failed
    }

    public class PaymentTransaction
    {
        [Key]
        public int PaymentId { get; set; }

        [ForeignKey("Repayment")]
        public int? RepaymentId { get; set; }

        [Required(ErrorMessage = "Application reference is required")]
        [ForeignKey("LoanApplication")]
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "Payment amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment gateway is required")]
        public PaymentGateway Gateway { get; set; }

        [Required(ErrorMessage = "Transaction reference is required")]
        [StringLength(100, ErrorMessage = "Transaction reference cannot exceed 100 characters")]
        public string TransactionRef { get; set; }

        [Required(ErrorMessage = "Payment status is required")]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [Required(ErrorMessage = "Paid date is required")]
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]

        public virtual Repayment? Repayment { get; set; }
        [JsonIgnore]
        public virtual LoanApplication? LoanApplication { get; set; }
    }
}
