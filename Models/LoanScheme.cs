using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
    public class LoanScheme
    {
        [Key]
        public int SchemeId { get; set; }

        [Required(ErrorMessage = "Scheme name is required")]
        [StringLength(100, ErrorMessage = "Scheme name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Minimum amount is required")]
        [Range(1000, 10000000, ErrorMessage = "Minimum amount must be between 1,000 and 10,000,000")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinAmount { get; set; }

        [Required(ErrorMessage = "Maximum amount is required")]
        [Range(1000, 10000000, ErrorMessage = "Maximum amount must be between 1,000 and 10,000,000")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxAmount { get; set; }

        [Required(ErrorMessage = "Interest rate is required")]
        [Range(1, 50, ErrorMessage = "Interest rate must be between 1% and 50%")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal InterestRate { get; set; }

        [Required(ErrorMessage = "Term in months is required")]
        [Range(1, 360, ErrorMessage = "Term must be between 1 and 360 months")]
        public int TermMonths { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required]
        public bool Active { get; set; } = true;
        [JsonIgnore]
        public virtual ICollection<LoanApplication>? LoanApplication { get; set; }
    }
}
