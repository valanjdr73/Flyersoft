using System;
namespace TopupBeneficiaries.Model
{
	public class ServiceUrlsConfig
	{
		private readonly ILogger<ServiceUrlsConfig> _logger;
		private readonly IConfiguration _configuration;
		public readonly string? FinanceServiceUrl;

		public ServiceUrlsConfig(ILogger<ServiceUrlsConfig> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;

			var serviceUrlSection = configuration.GetSection("ServiceUrls");
			if (serviceUrlSection != null)
			{
				FinanceServiceUrl = serviceUrlSection.GetValue<string>("FinanceService");
			}
			else
			{
				throw new SystemException("ServiceUrls not configured");
			}
		}
	}
}

