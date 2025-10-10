using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface ICustomerService
    {
        Task<Customer> Create(Customer customer);
        Task<Customer> Update(Customer customer);
        Task<Customer> Delete(int id);
        Task<Customer> GetById(int id);
        Task<IEnumerable<Customer>> GetAll();
        Task<bool> UpdateProfile(Customer updatedCustomer);
        public Task<Customer> VerifyKYC(int admin, int customerId, string aadhaarId);
    }
}
