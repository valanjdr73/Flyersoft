using System;
using TopupBeneficiaries.Entities;
using TopupBeneficiaries.Model;

namespace TopupBeneficiaries.Services
{
	public class UserFinanceService: IUserFinanceService
	{
        private readonly ILogger<UserFinanceService> _logger;
        private readonly ServiceUrlsConfig _serviceUrlsConfig;
		public UserFinanceService(ILogger<UserFinanceService> logger,
            ServiceUrlsConfig config)
		{
            _logger = logger;
            _serviceUrlsConfig = config;
    	}

        public bool DebitAmount(User user, decimal amountToDebit)
        {

            //HttpClient _serviceUrlsConfig.FinanceServiceUrl
            return true;
        }

        public decimal GetBalanceAmount(User user)
        {
            return 300m;
        }
    }
}

