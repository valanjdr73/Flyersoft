using System;
namespace TopupBeneficiaries.Model
{
	public interface ILimitsAndCharges
	{
        decimal TransactionCharges { get; }
        decimal VerifiedUserMonthlyLimit { get; }
        decimal UnVerifiedUserMonthlyLimit { get; }
        decimal TotalMonthlyLimit { get; }
    }
}

