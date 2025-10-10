using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Service
{
    public class RepaymentService : IRepaymentService
    {
        private readonly IRepaymentRepository _repos;
        private readonly ILoanApplicationRepository _appRepos;
        private readonly IEmailService _EmailService;
        public RepaymentService(IRepaymentRepository repos, ILoanApplicationRepository appRepos, IEmailService emailService)
        {
            _repos = repos;
            _appRepos = appRepos;
            _EmailService = emailService;

        }
        //public async Task<Repayment> PayInstallment(int applicationId, decimal paidAmount)
        //{
        //    if (paidAmount <= 0)
        //        throw new Exception("Paid amount should be greater than 0.");

        //    var repayments = (await _repos.GetAllByApplicationId(applicationId))
        //                      .OrderBy(r => r.InstallmentNumber)
        //                      .ToList();

        //    var application = await _appRepos.GetById(applicationId);
        //    if (application == null)
        //        throw new Exception("Loan application not found.");

        //    // Generate repayment schedule if none exists
        //    if (!repayments.Any())
        //    {
        //        await GenerateRepaymentSchedule(application);
        //        repayments = (await _repos.GetAllByApplicationId(applicationId))
        //                       .OrderBy(r => r.InstallmentNumber)
        //                       .ToList();
        //    }

        //    // Get first unpaid installment
        //    var currentRepayment = repayments.FirstOrDefault(r => r.AmountDue > 0);

        //    if (currentRepayment == null)
        //    {
        //        // Loan already fully paid
        //        application.Status = ApplicationStatus.Closed;
        //        await _appRepos.Update(application);
        //        throw new Exception("Loan fully repaid. No more installments left.");
        //    }

        //    // Partial or full EMI payment
        //    if (paidAmount >= currentRepayment.AmountDue)
        //    {
        //        // Full EMI paid
        //        decimal extraPayment = paidAmount - currentRepayment.AmountDue;

        //        currentRepayment.AmountPaid += currentRepayment.AmountDue;
        //        currentRepayment.AmountDue = 0;
        //        currentRepayment.Status = RepaymentStatus.Paid;
        //        currentRepayment.PaidDate = DateTime.UtcNow;

        //        await _repos.Update(currentRepayment);

        //        if (extraPayment > 0)
        //        {
        //            // Reduce principal by extra payment
        //            application.AppliedAmount -= extraPayment;
        //            if (application.AppliedAmount < 0) application.AppliedAmount = 0;
        //            await _appRepos.Update(application);

        //            // Delete old unpaid schedule and regenerate with remaining months
        //            int paidInstallments = repayments.Count(r => r.Status == RepaymentStatus.Paid);
        //            int remainingMonths = application.TermMonths - paidInstallments;

        //            if (remainingMonths > 0)
        //            {
        //                await _repos.DeleteAllByApplicationId(applicationId);
        //                await GenerateRepaymentSchedule(application, remainingMonths, paidInstallments);
        //            }
        //            else
        //            {
        //                // All installments cleared
        //                application.Status = ApplicationStatus.Closed;
        //                await _appRepos.Update(application);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // Partial EMI payment
        //        currentRepayment.AmountPaid += paidAmount;
        //        currentRepayment.AmountDue -= paidAmount;
        //        currentRepayment.Status = RepaymentStatus.Pending;
        //        await _repos.Update(currentRepayment);
        //    }

        //    // Check if all EMIs are paid → close loan
        //    bool allPaid = (await _repos.GetAllByApplicationId(applicationId))
        //                     .All(r => r.Status == RepaymentStatus.Paid);

        //    if (allPaid)
        //    {
        //        application.Status = ApplicationStatus.Closed;
        //        await _appRepos.Update(application);
        //    }

        //    return currentRepayment;
        //}

        public async Task<Repayment> PayInstallment(int applicationId)
        {
            var repayments = (await _repos.GetAllByApplicationId(applicationId))
                              .OrderBy(r => r.InstallmentNumber)
                              .ToList();

            var application = await _appRepos.GetById(applicationId);
            if (application == null)
                throw new Exception("Loan application not found.");

            // Generate repayment schedule if none exists
            if (!repayments.Any())
            {
                await GenerateRepaymentSchedule(application);
                repayments = (await _repos.GetAllByApplicationId(applicationId))
                               .OrderBy(r => r.InstallmentNumber)
                               .ToList();
            }

            // Get first unpaid installment
            var currentRepayment = repayments.FirstOrDefault(r => r.AmountDue > 0);
            if (currentRepayment == null)
            {
                // Loan fully repaid
                application.Status = ApplicationStatus.Closed;
                await _appRepos.Update(application);
                throw new Exception("Loan fully repaid. No more installments left.");
            }


            currentRepayment.AmountPaid = currentRepayment.AmountEMI;
            currentRepayment.AmountDue = 0;
            currentRepayment.Status = RepaymentStatus.Paid;
            currentRepayment.PaidDate = DateTime.UtcNow;


            await _repos.Update(currentRepayment);

            await SendRepaymentEmail(application, currentRepayment);

            bool allPaid = repayments.All(r => r.Status == RepaymentStatus.Paid);

            if (allPaid)
            {

                if (application != null && application.Status != ApplicationStatus.Closed)
                {
                    application.Status = ApplicationStatus.Closed;
                    await _appRepos.Update(application);

                    // Send Congratulations Mail
                    await SendClosureEmail(application);
                }
            }


            return currentRepayment;
        }



        private decimal CalculateEMI(decimal principal, decimal monthlyRate, int months)
        {
            decimal numerator = principal * monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), months);
            decimal denominator = (decimal)Math.Pow((double)(1 + monthlyRate), months) - 1;
            return Math.Round(numerator / denominator, 2);
        }

        private decimal CalculateDailyPenalty(Repayment repayment)
        {
            if (repayment.AmountDue <= 0)
                return 0m;

            int overdueDays = (DateTime.UtcNow - repayment.DueDate).Days;
            if (overdueDays <= 0)
                return 0m;

            decimal dailyPenaltyRate = 0.02m;
            return Math.Round(repayment.AmountDue * dailyPenaltyRate * overdueDays, 2);
        }


        async Task<Repayment> IRepaymentService.Delete(int id)
        {
            return await _repos.Delete(id);
        }
        async Task<IEnumerable<Repayment>> IRepaymentService.GetAll()
        {
            return await _repos.GetAll();
        }
        async Task<Repayment> IRepaymentService.GetById(int id)
        {
            return await _repos.GetById(id);
        }
        async Task<Repayment> IRepaymentService.Update(Repayment repayment)
        {
            return await _repos.Update(repayment);
        }

        //public async Task GenerateRepaymentSchedule(LoanApplication application, int remainingMonths = 0, int paidInstallments = 0)
        //{
        //    if (remainingMonths == 0) remainingMonths = application.TermMonths - paidInstallments;

        //    decimal principalRemaining = application.AppliedAmount;
        //    decimal monthlyRate = (decimal)application.InterestRate / 12 / 100;

        //    var newSchedule = new List<Repayment>();

        //    for (int i = 1; i <= remainingMonths; i++)
        //    {
        //        decimal emi = CalculateEMI(principalRemaining, monthlyRate, remainingMonths - i + 1);

        //        decimal interest = principalRemaining * monthlyRate;
        //        decimal principalComponent = emi - interest;
        //        principalRemaining -= principalComponent;
        //        if (principalRemaining < 0) principalRemaining = 0;

        //        var repayment = new Repayment
        //        {
        //            ApplicationId = application.ApplicationId,
        //            InstallmentNumber = paidInstallments + i,
        //            AmountEMI = emi,
        //            AmountPaid = 0,
        //            AmountDue = emi,
        //            DueDate = application.AppliedAt.AddMonths(paidInstallments + i),
        //            Status = RepaymentStatus.Pending,
        //            IsOverdue = false,
        //            PenaltyAmount = 0
        //        };

        //        newSchedule.Add(repayment);
        //    }

        //    // Save new schedule
        //    await _repos.BulkInsert(newSchedule);
        //}

        public async Task GenerateRepaymentSchedule(LoanApplication application, int remainingMonths = 0, int paidInstallments = 0)
        {
            if (remainingMonths == 0)
                remainingMonths = application.TermMonths - paidInstallments;

            decimal principal = application.AppliedAmount;
            decimal monthlyRate = (decimal)application.InterestRate / 12 / 100;

            // Calculate EMI (same amount each month)
            decimal emi = CalculateEMI(principal, monthlyRate, remainingMonths);

            var newSchedule = new List<Repayment>();

            for (int i = 1; i <= remainingMonths; i++)
            {
                var repayment = new Repayment
                {
                    ApplicationId = application.ApplicationId,
                    InstallmentNumber = paidInstallments + i,
                    AmountEMI = emi,              // fixed EMI
                    AmountPaid = 0,
                    AmountDue = emi,              // initially full EMI due
                    DueDate = application.AppliedAt.AddMonths(paidInstallments + i),
                    Status = RepaymentStatus.Pending,
                    IsOverdue = false,
                    PenaltyAmount = 0
                };

                newSchedule.Add(repayment);
            }

            await _repos.BulkInsert(newSchedule);
        }


        public async Task MarkOverdueRepaymentsAsync()
        {
            var repayments = await _repos.GetAll();

            foreach (var repayment in repayments)
            {
                if (repayment.Status == RepaymentStatus.Pending && repayment.DueDate.Date < DateTime.UtcNow.Date)
                {
                    repayment.Status = RepaymentStatus.Overdue;
                    repayment.IsOverdue = true;

                    // Optional: calculate penalty
                    repayment.PenaltyAmount = CalculateDailyPenalty(repayment);

                    await _repos.Update(repayment);
                }
            }
        }

        private async Task SendRepaymentEmail(LoanApplication application, Repayment repayment)
        {
            var customer = application.Customer;
            if (customer == null) return;

            var customerUser = customer.User;
            if (customerUser == null) return;

            string subject = "💰 Loan Repayment Successful";

            string body = $@"
      <html>
      <body style='font-family: Arial, sans-serif; color: #333;'>
      <h2 style='color: #28a745;'>Repayment Confirmation</h2>
      <p>Dear {customer.Customer_Name},</p>
      <p>We have successfully received your repayment for loan (Application ID: <b>{application.ApplicationId}</b>).</p>

      <p><b>Repayment Details:</b></p>
      <ul>
        <li>Installment Number: {repayment.InstallmentNumber}</li>
        <li>Amount Paid: ₹{repayment.AmountPaid}</li>
        <li>Payment Date: {repayment.PaidDate:dd-MMM-yyyy}</li>
        <li>Status: {repayment.Status}</li>
      </ul>
</p>
      <p style='margin-top:20px;'>Warm Regards,<br/>
      <p>Thank you for staying on track with your repayments, LoanMate Team</p>
      </body>
      </html>";

            try
            {
                await _EmailService.SendEmailAsync(customerUser.Email, subject, body);
                Console.WriteLine($"Repayment email sent to {customerUser.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send repayment email: {ex.Message}");
            }
        }



        private async Task SendClosureEmail(LoanApplication application)
        {
            var customer = application.Customer;
            if (customer == null) return;

            var customerUser = customer.User;
            if (customerUser == null) return;

            string subject = "🎉 Congratulations! Your Loan is Closed";

            string body = $@"
              <html>
              <body style='font-family: Arial, sans-serif; color: #333;'>
              <h2 style='color: #2e86c1;'>Loan Closure Confirmation</h2>
              <p>Dear {customer.Customer_Name},</p>
              <p>We are delighted to inform you that your loan (Application ID: <b>{application.ApplicationId}</b>) 
              has been <b>successfully closed</b> after your final repayment.</p>

              <p><b>Summary:</b></p>
              <ul>
              <li>Loan Amount: ₹{application.AppliedAmount}</li>
              <li>Scheme: {application.LoanScheme?.Name}</li>
              <li>Closed On: {DateTime.UtcNow:dd-MMM-yyyy}</li>
              </ul>

              <p>Thank you for being a valued customer. We look forward to serving you again.</p>
              <p style='margin-top:20px;'>Warm Regards,<br/>LoanMate Team</p>
              </body>
              </html>";

            try
            {
                await _EmailService.SendEmailAsync(customerUser.Email, subject, body);
                Console.WriteLine($"Closure email sent to {customerUser.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send closure email: {ex.Message}");
            }
        }



        public async Task SendRepaymentReminder()
        {
            var repayments = await _repos.GetAll();

            var upcomingPayment = repayments.Where(r => r.DueDate.Date == DateTime.UtcNow.AddDays(7).Date && r.Status != RepaymentStatus.Paid).ToList();
            //var upcomingPayment = repayments.Where(r=>r.Status != RepaymentStatus.Paid).ToList();

            foreach (var repayment in upcomingPayment)
            {
                var application = await _appRepos.GetById(repayment.ApplicationId);
                if (application != null)
                {

                    var customer = application.Customer; // Navigation property
                    if (customer == null) continue;

                    var customerUser = customer.User; // yaha se Email milega
                    if (customerUser == null) continue;
                    string subject = "Loan Repayment Reminder";
                    string body = $"Dear {customer.Customer_Name},<br/><br/>" +
                          $"Your loan repayment of ₹{repayment.AmountDue} " +
                          $"is due on {repayment.DueDate:dd-MMM-yyyy}.<br/>" +
                          $"Please pay on time to avoid penalties.";


                    try
                    {
                        await _EmailService.SendEmailAsync(customerUser.Email, subject, body);
                        Console.WriteLine($"Email sent to {customerUser.Email}");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email to {customerUser.Email}: {ex.Message}");
                        // Optionally log to DB
                    }
                }
            }

        }
    }
}
