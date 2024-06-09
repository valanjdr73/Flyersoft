using System;
namespace TopupBeneficiaries.Model
{
	public class LimitsAndCharges : ILimitsAndCharges
	{
        private readonly decimal _TransactionCharges;
        private readonly decimal _VerifiedUserMonthlyLimit;
        private readonly decimal _UnVerifiedUserMonthlyLimit;
        private readonly decimal _TotalMonthlyLimit;

        private readonly ILogger<ILimitsAndCharges> _logger;

        public LimitsAndCharges(ILogger<ILimitsAndCharges> logger, IConfiguration configuration)
		{
            _logger = logger;
            try
            {
                var limitsConfigSection = configuration.GetSection("LimitsAndCharges");
                if (limitsConfigSection != null)
                {
                    _TransactionCharges = limitsConfigSection.GetValue<decimal>("TransactionCharges");
                    _VerifiedUserMonthlyLimit = limitsConfigSection.GetValue<decimal>("VerifiedUserMonthlyLimit");
                    _UnVerifiedUserMonthlyLimit = limitsConfigSection.GetValue<decimal>("UnVerifiedUserMonthlyLimit");
                    _TotalMonthlyLimit = limitsConfigSection.GetValue<decimal>("TotalMonthlyLimit");
                }
                else
                {
                    _TransactionCharges = 1m;
                    _VerifiedUserMonthlyLimit = 500m;
                    _UnVerifiedUserMonthlyLimit = 1000m;
                    _TotalMonthlyLimit = 3000m;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading LimitsAndCharges Configuration");
                _TransactionCharges = 1m;
                _VerifiedUserMonthlyLimit = 500m;
                _UnVerifiedUserMonthlyLimit = 1000m;
                _TotalMonthlyLimit = 3000m;
            }
        }

        public decimal TransactionCharges => _TransactionCharges;

        public decimal VerifiedUserMonthlyLimit => _VerifiedUserMonthlyLimit;

        public decimal UnVerifiedUserMonthlyLimit => _UnVerifiedUserMonthlyLimit;

        public decimal TotalMonthlyLimit => _TotalMonthlyLimit;
    }
}

