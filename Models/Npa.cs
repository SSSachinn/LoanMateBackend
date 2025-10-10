using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.Models
{
    public class Npa
    {
        [Key]
        public int NpaId { get; set; }

        [Required(ErrorMessage = "Customer reference is required")]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Loan Application is required")]
        [ForeignKey("LoanApplication")]
        public int LoanApplicationId { get; set; } //per loan total overdue

        public decimal TotalOverdue { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public virtual LoanApplication? LoanApplication { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
