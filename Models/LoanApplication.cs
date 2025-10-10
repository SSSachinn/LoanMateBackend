using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
    public enum ApplicationStatus
    {
        Pending,
        UnderReview,
        Approved,
        Rejected,
        Disbursed,
        Closed
    }

    public class LoanApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "Customer reference is required")]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Loan scheme reference is required")]
        [ForeignKey("LoanScheme")]
        public int SchemeId { get; set; }

        [Required(ErrorMessage = "Applied amount is required")]
        [Range(1000, 10000000, ErrorMessage = "Applied amount must be between 1,000 and 10,000,000")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AppliedAmount { get; set; }

        [Required(ErrorMessage = "Term in months is required")]
        [Range(1, 360, ErrorMessage = "Term must be between 1 and 360 months")]
        public int TermMonths { get; set; }

        [Range(1, 50, ErrorMessage = "Interest rate must be between 1% and 50%")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? InterestRate { get; set; }

        [Required(ErrorMessage = "Application status is required")]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

        [ForeignKey("LoanOfficer")]
        public int? AssignedOfficerId { get; set; }

        [Required(ErrorMessage = "Applied date is required")]
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DecisionAt { get; set; }
        public string? RejectionReason { get; set; }
        public bool IsNpa { get; set; } = false;
        public virtual Customer? Customer { get; set; }
        [JsonIgnore]
        public virtual LoanScheme? LoanScheme { get; set; }
        public virtual LoanOfficer? LoanOfficer { get; set; }
        [JsonIgnore]
        public virtual ICollection<Document>? Document { get; set; }
        [JsonIgnore]
        public virtual ICollection<Repayment>? Repayment { get; set; }
        [JsonIgnore]
        public virtual ICollection<PaymentTransaction>? PaymentTransaction { get; set; }
    }
}
