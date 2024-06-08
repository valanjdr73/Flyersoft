using System;
using TopupBeneficiaries.Entities;

namespace TopupBeneficiaries.Repositories
{
	public interface IUserRepository
	{
		Task<List<TopUpBeneficiary>> GetActiveBeneficiariesAsync(long userId);
		bool AddBeneficiary(TopUpBeneficiary beneficiary, User user);
		Task<int> GetActiveBeneficiaryCountAsync(long userId);
		Task<User?> GetUserAndBeneficiariesAsync(long userId);
		Task<TopUpBeneficiary?> GetBeneficiaryDetails(long beneficiaryId);
		Task<List<TopUpTransaction>> GetUserTransactionsByDateRangeAsync(long userId, DateTime startDate, DateTime endDate);
	}
}

