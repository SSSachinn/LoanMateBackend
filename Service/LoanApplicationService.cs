using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace LoanManagementSystem.Service
{
    public class LoanApplicationService : ILoanApplicationService
    {
        private readonly ILoanApplicationRepository _repos;
        private readonly ILoanOfficerService _officerService;
        private readonly IEmailService _emailService;
        private readonly ILoanSchemeService _schemeService;
        private readonly LoanManagementSystemContext _context;

        public LoanApplicationService(ILoanApplicationRepository repos, ILoanOfficerService officerService, IEmailService emailService,ILoanSchemeService schemeService,LoanManagementSystemContext context)
        {
            _repos = repos;
            _officerService = officerService;
            _emailService = emailService;
            _schemeService = schemeService;
            _context = context;

        }

        public async Task<LoanApplication> Create(LoanApplication application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application), "Application cannot be null");

            application.Status = ApplicationStatus.Pending;
            application.AppliedAt = DateTime.UtcNow;

            application.AssignedOfficerId = null;
            // 1️⃣ Save application first
            var createdApplication = await _repos.Create(application);

            // 2️⃣ Assign officer now that ApplicationId exists
            var officer = await _officerService.AssignApplication(createdApplication.ApplicationId);
            createdApplication.AssignedOfficerId = officer.OfficerId;

            //createdApplication.InterestRate = application.LoanScheme.InterestRate;
            var scheme = await _schemeService.GetById(createdApplication.SchemeId);

            if (scheme != null)
            {
                createdApplication.InterestRate = scheme.InterestRate;


            }

            // 3️⃣ Save again
            await _repos.SaveChangesAsync();

            return createdApplication;
        }

        async Task<LoanApplication> ILoanApplicationService.Delete(int id)
        {
            return await _repos.Delete(id);
        }
        async Task<IEnumerable<LoanApplication>> ILoanApplicationService.GetAll()
        {
            return await _repos.GetAll();
        }
        async Task<LoanApplication> ILoanApplicationService.GetById(int id)
        {
            return await _repos.GetById(id);
        }
        async Task<LoanApplication> ILoanApplicationService.Update(LoanApplication application)
        {
            return await _repos.Update(application);
        }


        public async Task<List<DailyLoanCount>> GetDailyLoanCountsAsync(DateTime? start = null, DateTime? end = null)
        {
            var query = _context.LoanApplications.AsQueryable();

            if (start.HasValue)
                query = query.Where(x => x.AppliedAt >= start.Value.Date);
            if (end.HasValue)
                query = query.Where(x => x.AppliedAt <= end.Value.Date);

            var dailyCounts = await query
                .GroupBy(x => x.AppliedAt.Date)
                .Select(g => new DailyLoanCount
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            return dailyCounts;
        }



        async Task<LoanApplication> ILoanApplicationService.Approve(int applicationId,int officerId)
        {
            var application = await _repos.GetById(applicationId);
            await ValidateOfficerAssigned(application);
            if (application == null || application.Status != ApplicationStatus.Pending)
            {
                throw new InvalidOperationException("Application not found or not in a Pending state.");
            }
            application.Status = ApplicationStatus.Approved;
            application.DecisionAt = DateTime.UtcNow;

            // Updating all attached documents
            if (application.Document != null)
            {
                foreach (var doc in application.Document)
                {
                    doc.VerifiedBy = officerId;
                    doc.VerificationStatus = VerificationStatus.Verified;
                }
            }

            await _repos.SaveChangesAsync();

            // Send email to customer
            var customerEmail = application.Customer?.User?.Email;     //customers email coming
            if (!string.IsNullOrEmpty(customerEmail))
            {
                var subject = $"Your Loan Application #{application.ApplicationId} is Approved!";
                var body = $"Hello {application.Customer.Customer_Name},<br><br>" +
                           $"Your loan application has been <b>approved</b>.<br>" +
                           $"Applied Amount: {application.AppliedAmount:C}<br>" +
                           $"Term: {application.TermMonths} months<br>" +
                           $"Interest Rate: {application.InterestRate}%<br><br>" +
                           $"Thank you!!.";
                try
                {
                 await _emailService.SendEmailAsync(customerEmail, subject, body);

                }
                catch (Exception ex)
                {
                    // Log to console or file
                    Console.WriteLine("Email sending failed: " + ex.Message);
                    throw;
                }

            }

            return application;
        }
        async Task<LoanApplication> ILoanApplicationService.Reject(int applicationId, string reason,int officerId)
        {
            var application = await _repos.GetById(applicationId);
            await ValidateOfficerAssigned(application);
            if (application == null || application.Status != ApplicationStatus.Pending)
            {
                throw new InvalidOperationException("Application not found or not in a Pending state.");
            }
            application.Status = ApplicationStatus.Rejected;
            application.RejectionReason = reason;
            application.DecisionAt = DateTime.UtcNow;

            // Updating all attached documents
            if (application.Document != null)
            {
                foreach (var doc in application.Document)
                {
                    doc.VerifiedBy = officerId;
                    doc.VerificationStatus = VerificationStatus.Rejected;
                }
            }

            await _repos.SaveChangesAsync();

            // Send email to customer
            var customerEmail = application.Customer?.User?.Email;
            if (!string.IsNullOrEmpty(customerEmail))
            {
                var subject = $"Your Loan Application #{application.ApplicationId} is Rejected";
                var body = $"Hello {application.Customer.Customer_Name},<br><br>" +
                           $"Your loan application has been <b>rejected</b>.<br>" +
                           $"Reason: {reason}<br><br>" +
                           $"For any queries, please contact support.";
                await _emailService.SendEmailAsync(customerEmail, subject, body);
            }

            return application;
        }
        async Task<LoanApplication> ILoanApplicationService.Close(int applicationId)
        {
            var application = await _repos.GetById(applicationId);
            await ValidateOfficerAssigned(application);
            if (application == null || application.Status != ApplicationStatus.Disbursed)
            {
                throw new InvalidOperationException("Application not found or not in an approved state.");
            }
            application.Status = ApplicationStatus.Closed;
            await _repos.SaveChangesAsync();
            return application;
        }
        async Task<LoanApplication> ILoanApplicationService.Disburse(int applicationId)
        {
            var application = await _repos.GetById(applicationId);
            await ValidateOfficerAssigned(application);
            if (application == null || application.Status != ApplicationStatus.Approved)
            {
                throw new InvalidOperationException("Application not found or not in an approved state.");
            }
            application.Status = ApplicationStatus.Disbursed;
            await _repos.SaveChangesAsync();
            return application;
        }
        async Task<LoanApplication> ILoanApplicationService.UnderReview(int applicationId)
        {
            var application = await _repos.GetById(applicationId);
            await ValidateOfficerAssigned(application);
            if (application == null || application.Status != ApplicationStatus.Pending)
            {
                throw new InvalidOperationException("Application not found or not in a pending state.");
            }
            application.Status = ApplicationStatus.UnderReview;
            await _repos.SaveChangesAsync();
            return application;
        }


        private async Task ValidateOfficerAssigned(LoanApplication application)
        {
            if (application == null)
                throw new InvalidOperationException("Application not found.");

            if (application.AssignedOfficerId == null)
                throw new InvalidOperationException("No officer assigned to this application.");

            var officer = await _officerService.GetById(application.AssignedOfficerId.Value);
            if (officer == null || !officer.Active)
                throw new UnauthorizedAccessException("Assigned officer is not active.");
        }

    }
}