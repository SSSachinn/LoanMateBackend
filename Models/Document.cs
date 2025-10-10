using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.Models
{
    public enum DocumentType
    {
        Aadhaar,
        PAN,
        SalarySlip,
        BankStatement
    }

    public enum VerificationStatus
    {
        Pending,
        Verified,
        Rejected
    }

    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Required(ErrorMessage = "Application reference is required")]
        [ForeignKey("LoanApplication")]
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "Document type is required")]
        public DocumentType DocType { get; set; }

        [Required(ErrorMessage = "File path is required")]
        [StringLength(500, ErrorMessage = "File path cannot exceed 500 characters")]
        public string FilePath { get; set; }

        [Required(ErrorMessage = "Upload date is required")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public int? VerifiedBy { get; set; }

        [Required(ErrorMessage = "Verification status is required")]
        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;

        public virtual LoanApplication? LoanApplication { get; set; }
    }
}
