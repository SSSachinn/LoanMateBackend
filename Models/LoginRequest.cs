using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username Required")]
        [StringLength(15, ErrorMessage = "User Name cannot be longer than 15 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
