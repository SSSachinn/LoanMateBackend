using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
    public enum Role
    {
        Admin,
        LoanOfficer,
        Customer
    }
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>]).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase, one lowercase, one digit, and one special character."
        )]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public Role Role { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public virtual ICollection<Notification>? Notification { get; set; }
        [JsonIgnore]
        public virtual ICollection<Report>? Report { get; set; }
    }
}

