using System;
using System.Threading.Tasks;
using TopupBeneficiaries.DBContext;
using TopupBeneficiaries.Entities;
using Microsoft.EntityFrameworkCore;
using TopupBeneficiaries.Model;

namespace TopupBeneficiaries.Repositories
{
	public class TopUpRepository: ITopUpRepository
	{
        private readonly TopupBeneficiaryContext _dbContext;
        private readonly ILogger<TopUpRepository> _logger;
        //private readonly LimitsAndCharges limitsSettings;
        
		public TopUpRepository(ILogger<TopUpRepository> logger,
            TopupBeneficiaryContext context)
		{
            _dbContext = context;
            _logger = logger;
            //limitsSettings = limitsAndCharges;
		}

        public async Task<List<TopUpOption>> GetAllTopUpOptionsAsync()
        {
            return  await _dbContext.topUpOptions.Where(t => t.IsDeleted == false).ToListAsync();          
        }

        public async Task<TopUpOption?> GetTopUpOptionByIdAsync(int topupOptionId)
        {
            return await _dbContext.topUpOptions.FirstOrDefaultAsync(t => t.Id == topupOptionId && t.IsDeleted == false);
        }

        public async Task<TopUpTransaction?> BuildTransactionAsync(long beneficiaryId)
        {
            return await _dbContext.TopUpBeneficiaries.Include(a => a.User)
                .Select(b => new TopUpTransaction {
                    TopUpBeneficiaryId = b.Id,
                    TopUpBeneficiary = b,
                    UserId = b.UserId,
                    User = b.User,
                })
                .FirstOrDefaultAsync(t => t.TopUpBeneficiaryId == beneficiaryId && t.TopUpBeneficiary.IsDeleted == false && t.TopUpBeneficiary.IsActive == true);
        }

        public async Task<bool> SaveTopUpTransactionAsync(TopUpTransaction topUpTransaction)
        {
            try
            {
                await _dbContext.TopUpTransactions.AddAsync(topUpTransaction);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a TopUp transaction");
                throw new Exception("Error adding a Topup transaction",ex);
            }
        }
    }
}

