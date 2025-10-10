using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Data
{
    public class LoanManagementSystemContext:DbContext
    {
        public LoanManagementSystemContext(DbContextOptions<LoanManagementSystemContext> options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<LoanOfficer> LoanOfficers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LoanScheme> LoanSchemes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<PaymentTransaction> PaymentsTransactions { get; set; }
        public DbSet<Repayment> Repayments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Npa> Npas { get; set; }

    }
}
