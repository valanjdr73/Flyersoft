using System;
namespace TopupBeneficiaries.Model
{
	public class LimitsAndCharges
	{
        public readonly decimal TransactionCharges;
        public readonly decimal VerifiedUserMonthlyLimit;
        public readonly decimal UnVerifiedUserMonthlyLimit;
        public readonly decimal TotalMonthlyLimit;

        private readonly ILogger<LimitsAndCharges> _logger;

        public LimitsAndCharges(ILogger<LimitsAndCharges> logger, IConfiguration configuration)
		{
            _logger = logger;
            try
            {
                var limitsConfigSection = configuration.GetSection("LimitsAndCharges");
                if (limitsConfigSection != null)
                {
                    TransactionCharges = limitsConfigSection.GetValue<decimal>("TransactionCharges");
                    VerifiedUserMonthlyLimit = limitsConfigSection.GetValue<decimal>("VerifiedUserMonthlyLimit");
                    UnVerifiedUserMonthlyLimit = limitsConfigSection.GetValue<decimal>("UnVerifiedUserMonthlyLimit");
                    TotalMonthlyLimit = limitsConfigSection.GetValue<decimal>("TotalMonthlyLimit");
                }
                else
                {
                    TransactionCharges = 1m;
                    VerifiedUserMonthlyLimit = 500m;
                    UnVerifiedUserMonthlyLimit = 1000m;
                    TotalMonthlyLimit = 3000m;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading LimitsAndCharges Configuration");
                TransactionCharges = 1m;
                VerifiedUserMonthlyLimit = 500m;
                UnVerifiedUserMonthlyLimit = 1000m;
                TotalMonthlyLimit = 3000m;
            }
        }

	}
}

