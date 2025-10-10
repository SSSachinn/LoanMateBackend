using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required(ErrorMessage = "GeneratedBy user is required")]
        [ForeignKey("User")]
        public int GeneratedBy { get; set; }

        [Required(ErrorMessage = "Report type is required")]
        [StringLength(100, ErrorMessage = "Report type cannot exceed 100 characters")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Generated date is required")]
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "File path is required")]
        [StringLength(500, ErrorMessage = "File path cannot exceed 500 characters")]
        public string FilePath { get; set; }

        public virtual User? User { get; set; }
    }
}
