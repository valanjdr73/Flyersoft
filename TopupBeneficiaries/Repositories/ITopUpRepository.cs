using System;
using TopupBeneficiaries.Entities;

namespace TopupBeneficiaries.Repositories
{
	public interface ITopUpRepository
	{
		Task<List<TopUpOption>> GetAllTopUpOptionsAsync();
		Task<TopUpOption?> GetTopUpOptionByIdAsync(int topupOptionId);
		Task<TopUpTransaction?> BuildTransactionAsync(long beneficiaryId);
		Task<bool> SaveTopUpTransactionAsync(TopUpTransaction topUpTransaction);
	}
}

