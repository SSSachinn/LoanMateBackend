using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repos;
        private readonly IUserRepository _userRepos;
        public CustomerService(ICustomerRepository repos, IUserRepository userRepos)
        {
            _repos = repos;
            _userRepos = userRepos;
        }
        async Task<Customer> ICustomerService.Create(Customer customer)
        {
            return await _repos.Create(customer);
        }

        async Task<Customer> ICustomerService.Delete(int id)
        {
            return await _repos.Delete(id);
        }

        async Task<IEnumerable<Customer>> ICustomerService.GetAll()
        {
            return await _repos.GetAll();
        }

        async Task<Customer> ICustomerService.GetById(int id)
        {
            return await _repos.GetById(id);
        }

        async Task<Customer> ICustomerService.Update(Customer customer)
        {
            return await _repos.Update(customer);
        }
        public async Task<bool> UpdateProfile(Customer updatedCustomer)
        {
            var existing = await _repos.GetById(updatedCustomer.Customer_Id);
            if (existing == null)
                return false;

            existing.Customer_Name = updatedCustomer.Customer_Name;
            existing.Address = updatedCustomer.Address;
            if (existing.User != null && updatedCustomer.User != null)
            {
                existing.User.Phone = updatedCustomer.User.Phone;
            }

            await _repos.SaveChangesAsync();
            return true;
        }
        public async Task<Customer> VerifyKYC(int adminId, int customerId, string aadhaarId)
        {
            // 1. Fetch the admin from User repository
            var admin = await _userRepos.GetById(adminId);
            if (admin == null || admin.Role != Role.Admin)
                throw new UnauthorizedAccessException("Only Admin can verify KYC.");

            // 2. Fetch the customer
            var customer = await _repos.GetById(customerId);
            if (customer == null)
                return null;

            // 3. Verify or reject based on Aadhaar
            customer.KYCStatus = customer.AadhaarID == aadhaarId
                                 ? KYCStatus.Verified
                                 : KYCStatus.Rejected;

            // 4. Record which admin verified
            customer.VerifiedByAdminId = admin.UserId;

            // 5. Save changes
            await _repos.SaveChangesAsync();

            return customer;
        }

    }
}
