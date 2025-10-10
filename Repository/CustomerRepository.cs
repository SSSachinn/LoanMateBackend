using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly LoanManagementSystemContext _context;
        public CustomerRepository(LoanManagementSystemContext context)
        {
            _context = context;
        }
        async Task<Customer> ICustomerRepository.Create(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        async Task<Customer> ICustomerRepository.Delete(int id)
        {
            var existing = await _context.Customers.FirstOrDefaultAsync(c => c.Customer_Id == id);
            if (existing != null)
            {
                _context.Customers.Remove(existing);
                await _context.SaveChangesAsync();
            }
            return existing;
        }

        async Task<IEnumerable<Customer>> ICustomerRepository.GetAll()
        {
            return await _context.Customers.Include(u=>u.User).ToListAsync();   
        }

        async Task<Customer> ICustomerRepository.GetById(int id)
        {
            var existing = _context.Customers.Include(u => u.User).FirstOrDefault(c => c.Customer_Id == id);
            if(existing == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }   
            return existing;
        }

        public async Task<Customer> Update(Customer customer)
        {
            var existing = await _context.Customers.FirstOrDefaultAsync(c => c.Customer_Id == customer.Customer_Id);
            if (existing != null)
            {
                existing.Customer_Id= customer.Customer_Id; 
                existing.CustomerUserId= customer.CustomerUserId;
                existing.Customer_Name= customer.Customer_Name;
                existing.DOB= customer.DOB;
                existing.Address= customer.Address;
                existing.City= customer.City;
                existing.AadhaarID= customer.AadhaarID;
                existing.KYCStatus= customer.KYCStatus;
                await _context.SaveChangesAsync();
                return existing;
            }
            return null;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
