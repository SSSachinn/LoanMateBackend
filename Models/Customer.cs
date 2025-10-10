using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace LoanManagementSystem.Models
{
    public enum KYCStatus
    {
        Pending,
        Verified,
        Rejected
    }
    public class Customer
    {
        [Key]
        public int Customer_Id { get; set; }

        [Required(ErrorMessage = "User reference is required")]
        [ForeignKey("User")]
        public int CustomerUserId { get; set; }
        public int? VerifiedByAdminId { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters")]
        public string Customer_Name { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "Aadhar ID is required")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhar ID must be a 12-digit number")]
        public string AadhaarID { get; set; }

        [Required(ErrorMessage = "KYC status is required")]
        public KYCStatus KYCStatus { get; set; }
        
        public virtual User? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<LoanApplication>? LoanApplication { get; set; }
    }
}
