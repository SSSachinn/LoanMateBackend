using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
    public enum RepaymentStatus
    {
        Pending,
        Paid,
        Overdue,
        Defaulted,
        Failed
    }

    public class Repayment
    {
        [Key]
        public int RepaymentId { get; set; }

        [Required(ErrorMessage = "Application reference is required")]
        [ForeignKey("LoanApplication")]
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "Installment number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Installment number must be greater than 0")]
        public int InstallmentNumber { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        [Required(ErrorMessage = "Amount due is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount due cannot be negative")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }
        [Required(ErrorMessage = "Amount due is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount due cannot be negative")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountEMI { get; set; }

        [Required(ErrorMessage = "Amount due is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount due cannot be negative")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountDue { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? PaidDate { get; set; }

        [Required(ErrorMessage = "Repayment status is required")]
        public RepaymentStatus Status { get; set; } = RepaymentStatus.Pending;

        [Required]
        public bool IsOverdue { get; set; } = false;

        [Range(0, double.MaxValue, ErrorMessage = "Penalty amount cannot be negative")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PenaltyAmount { get; set; } = 0;
        [JsonIgnore]
        public virtual LoanApplication? LoanApplication { get; set; }
        [JsonIgnore]
        public virtual ICollection<PaymentTransaction>? PaymentTransaction { get; set; }
    }
}
