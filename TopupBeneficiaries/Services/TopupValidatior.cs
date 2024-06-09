using System;
using TopupBeneficiaries.Entities;
using TopupBeneficiaries.Model;

namespace TopupBeneficiaries.Services
{
	public class TopupValidatior: ITopupValidator
	{
        private readonly ILogger<TopupValidatior> _logger;
        private readonly ILimitsAndCharges limitSettings;

		public TopupValidatior(ILogger<TopupValidatior> logger, ILimitsAndCharges limitAnCharges)
		{
            _logger = logger;
            limitSettings = limitAnCharges;
		}

        public bool ValidateUnVerifiedUserTopup(User user, TopUpBeneficiary beneficiary, TopUpOption topUpOption, List<TopUpTransaction> topUpTransactions)
        {
            if (topUpTransactions == null || topUpTransactions.Count() == 0)
            {
                return true;
            }
            else
            {
                return topUpTransactions.Where(c => c.TopUpBeneficiaryId == beneficiary.Id).Sum(t => t.TopUpOption.TopUpAmount) + topUpOption.TopUpAmount <= limitSettings.UnVerifiedUserMonthlyLimit;
            }
        }


        public bool ValidateUserTopupLimit(User user, TopUpOption topUpOption, List<TopUpTransaction> topUpTransactions)
        {
            if (topUpTransactions == null || topUpTransactions.Count() == 0)
            {
                return true;
            }
            else
            {
                return topUpTransactions.Sum(t => t.TopUpOption.TopUpAmount) + topUpOption.TopUpAmount <= limitSettings.TotalMonthlyLimit;
            }
        }


        public bool ValidateVerifiedUserTopup(User user, TopUpBeneficiary beneficiary, TopUpOption topUpOption, List<TopUpTransaction> topUpTransactions)
        {
            if (topUpTransactions == null || topUpTransactions.Count() == 0)
            {
                return true;
            }
            else
            {
                var sum = topUpTransactions.Where(c => c.TopUpBeneficiaryId == beneficiary.Id).Sum(t => t.TopUpOption.TopUpAmount);
                return sum + topUpOption.TopUpAmount <= limitSettings.VerifiedUserMonthlyLimit;
            }
        }
    }
}

