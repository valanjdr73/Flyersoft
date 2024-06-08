using System;
using TopupBeneficiaries.Entities;

namespace TopupBeneficiaries.Services
{
	public interface IUserFinanceService
	{
		decimal GetBalanceAmount(User user);
		bool DebitAmount(User user, decimal amountToDebit);
	}
}

