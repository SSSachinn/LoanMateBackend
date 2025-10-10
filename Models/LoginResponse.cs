namespace LoanManagementSystem.Models
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
    }
}
