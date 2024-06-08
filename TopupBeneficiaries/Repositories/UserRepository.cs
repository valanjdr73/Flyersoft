using System;
using System.Linq;
using TopupBeneficiaries.DBContext;
using TopupBeneficiaries.Entities;
using Microsoft.EntityFrameworkCore;

namespace TopupBeneficiaries.Repositories
{
	public class UserRepository : IUserRepository
	{
        private readonly TopupBeneficiaryContext _dbContext;
        private readonly ILogger<UserRepository> _logger;
		public UserRepository(ILogger<UserRepository> logger, TopupBeneficiaryContext context)
		{
            _dbContext = context;
            _logger = logger;
		}

        public async Task<List<TopUpBeneficiary>> GetActiveBeneficiariesAsync(long userId)
        {
            List<TopUpBeneficiary> beneficiaries = new List<TopUpBeneficiary>();
            beneficiaries = await _dbContext.TopUpBeneficiaries.Include(i => i.User).Where(t => t.UserId== userId && t.IsDeleted == false && t.IsActive == true).ToListAsync();
            return beneficiaries;
        }

        public async Task<User?> GetUserAndBeneficiariesAsync(long userId)
        {
            return  await _dbContext.Users.Include(t => t.TopUpBeneficiaries).FirstOrDefaultAsync(x => x.Id == userId && x.IsDeleted== false);
        }

        public bool AddBeneficiary(TopUpBeneficiary beneficiary, User user)
        {

            user?.TopUpBeneficiaries?.Add(beneficiary); 
            return _dbContext.SaveChanges() > 0;
            
        }

        public async Task<TopUpBeneficiary?> GetBeneficiaryDetails(long beneficiaryId)
        {
            return await _dbContext.TopUpBeneficiaries.Include(t => t.User).FirstOrDefaultAsync(b => b.Id == beneficiaryId && b.IsDeleted == false && b.IsActive == true);
        }

        public async Task<int> GetActiveBeneficiaryCountAsync(long userId)
        {
            return await _dbContext.TopUpBeneficiaries.Where(b => b.UserId == userId && b.IsActive == true && b.IsDeleted == false).CountAsync();
        }

        public async Task<List<TopUpTransaction>> GetUserTransactionsByDateRangeAsync(long userId, DateTime startDate, DateTime endDate)
        {
            return await _dbContext.TopUpTransactions
                .Include(o => o.TopUpOption)
                .Include(b => b.TopUpBeneficiary)
                .Where(c => c.UserId == userId && c.TopupDateTime >= startDate && c.TopupDateTime <= endDate)
                .ToListAsync();
        }
    }
}

