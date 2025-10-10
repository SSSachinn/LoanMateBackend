using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class LoanSchemeService: ILoanSchemeService
    {
        private readonly ILoanSchemeRepository _repos;
        private readonly IUserRepository _userRepos;
        public LoanSchemeService(ILoanSchemeRepository repos, IUserRepository userRepos)
        {
            _repos = repos;
            _userRepos = userRepos;
        }
        async Task<LoanScheme> ILoanSchemeService.Create(LoanScheme scheme)
        {
            return await _repos.Create(scheme);
        }
        async Task<LoanScheme> ILoanSchemeService.Delete(int id)
        {
            return await _repos.Delete(id);
        }
        async Task<IEnumerable<LoanScheme>> ILoanSchemeService.GetAll()
        {
            return await _repos.GetAll();
        }
        async Task<LoanScheme> ILoanSchemeService.GetById(int id)
        {
            return await _repos.GetById(id);
        }
        async Task<LoanScheme> ILoanSchemeService.Update(LoanScheme scheme)
        {
            return await _repos.Update(scheme);
        }
        async Task<bool> ILoanSchemeService.Activate(int adminId, int schemeId)
        {
            var admin = await _userRepos.GetById(adminId);
            if (admin == null || admin.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException("Only Admin can activate a loan scheme.");
            }
            var scheme = await _repos.GetById(schemeId);
            if (scheme == null)
            {
                return false;
            }
            scheme.Active = true;
            await _repos.SaveChangesAsync();
            return true;
        }
        async Task<bool> ILoanSchemeService.Deactivate(int adminId, int schemeId)
        {
            var admin = await _userRepos.GetById(adminId);
            if (admin == null || admin.Role != Role.Admin)
            {
                throw new UnauthorizedAccessException("Only Admin can activate a loan scheme.");
            }
            var scheme = await _repos.GetById(schemeId);
            if (scheme == null)
            {
                return false;
            }
            scheme.Active = false;
            await _repos.SaveChangesAsync();
            return true;
        }
        async Task<decimal> ILoanSchemeService.CalculateInterest(int schemeId, decimal amount)
        {
            var scheme = await _repos.GetById(schemeId);
            if (scheme == null || !scheme.Active)
            {
                throw new InvalidOperationException("Invalid or inactive loan scheme.");
            }
            decimal years = scheme.TermMonths / 12m;
            return amount * scheme.InterestRate *years/ 100;
        }

    }
}
