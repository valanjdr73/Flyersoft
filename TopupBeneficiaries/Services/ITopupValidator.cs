using System;
using TopupBeneficiaries.Entities;

namespace TopupBeneficiaries.Services
{
	public interface ITopupValidator
	{
		bool ValidateVerifiedUserTopup(User user, TopUpBeneficiary beneficiary, TopUpOption topUpOption, List<TopUpTransaction> topUpTransactions);
		bool ValidateUnVerifiedUserTopup(User user, TopUpBeneficiary beneficiary, TopUpOption topUpOption, List<TopUpTransaction> topUpTransactions);
		bool ValidateUserTopupLimit(User user, TopUpOption topUpOption, List<TopUpTransaction> topUpTransactions);
    }
}

