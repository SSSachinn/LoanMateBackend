using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class LoanOfficerService: ILoanOfficerService
    {
        private readonly ILoanOfficerRepository _repos;
        private readonly ILoanApplicationRepository _applicationRepos;
        private readonly IUserRepository _userRepos;

        public LoanOfficerService(
            ILoanOfficerRepository repos,
            ILoanApplicationRepository applicationRepos,
            IUserRepository userRepos)
        {
            _repos = repos;
            _applicationRepos = applicationRepos;
            _userRepos = userRepos;
        }
        public async Task<LoanOfficer> Create(LoanOfficer officer)
        {
            var user = await _userRepos.GetById(officer.UserId); 
            if (user == null)
                throw new ArgumentException("User not found");

            if (user.Role != Role.LoanOfficer) 
                throw new UnauthorizedAccessException("❌ Only LoanOfficer User Can become officers");

            return await _repos.Create(officer);
        }
        async Task<LoanOfficer> ILoanOfficerService.Delete(int id)
        {
            return await _repos.Delete(id);
        }
        async Task<IEnumerable<LoanOfficer>> ILoanOfficerService.GetAll()
        {
            return await _repos.GetAll();
        }
        async Task<LoanOfficer> ILoanOfficerService.GetById(int id)
        {
            return await _repos.GetById(id);
        }
        async Task<LoanOfficer> ILoanOfficerService.Update(LoanOfficer officer)
        {
            return await _repos.Update(officer);
        }
        async Task<LoanOfficer> ILoanOfficerService.GetByUserId(int userId)
        {
            return await _repos.GetByUserId(userId);
        }
        public async Task<LoanOfficer> AssignApplication(int applicationId)
        {
            // 1️⃣ Get the loan application including customer
            var application = await _applicationRepos.GetById(applicationId);
            if (application == null)
                throw new ArgumentException("Invalid application ID");

            // 2️⃣ Get customer city
            var customerCity = application.Customer?.City;
            if (string.IsNullOrEmpty(customerCity))
                throw new InvalidOperationException("Customer city not available");

            // 3️⃣ Get officers in the same city
            var cityOfficers = await _repos.GetByCityAsync(customerCity);

            LoanOfficer selectedOfficer;
            var random = new Random();

            if (cityOfficers != null && cityOfficers.Any())
            {
                // Pick a random officer from the same city
                selectedOfficer = cityOfficers[random.Next(cityOfficers.Count)];
            }
            else
            {
                // No officer in same city → pick random from all active officers
                var activeOfficers = await _repos.GetActiveOfficersAsync();
                if (activeOfficers == null || !activeOfficers.Any())
                    throw new InvalidOperationException("No active loan officers available");

                selectedOfficer = activeOfficers[random.Next(activeOfficers.Count)];
            }

            // 4️⃣ Assign the selected officer to the application
            application.AssignedOfficerId = selectedOfficer.OfficerId;

            // 5️⃣ Save changes
            await _applicationRepos.SaveChangesAsync();

            return selectedOfficer;
        }

        async Task<bool> ILoanOfficerService.Activate(int officerId)
        {
            
            var officer = await _repos.GetById(officerId);
            if(officer == null)
            {
                return false;
            }
            officer.Active = true;
            await _repos.SaveChangesAsync();
            return true;
        }
        async Task<bool> ILoanOfficerService.Deactivate(int officerId)
        {
            
            var officer = await _repos.GetById(officerId);
            if (officer == null)
            {
                return false;
            }
            officer.Active = false;
            await _repos.SaveChangesAsync();
            return true;
        }



    }
}
