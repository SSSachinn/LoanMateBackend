using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
    public class LoanOfficer
    {
        [Key]
        public int OfficerId { get; set; } 

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; } 

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string City { get; set; }

        [Required]
        public bool Active { get; set; } = true;
        public virtual User? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<LoanApplication>? LoanApplication { get; set; }
    }
}

