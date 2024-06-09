using System;
using Microsoft.Extensions.Logging;
using Moq;
using TopupBeneficiaries.Entities;
using TopupBeneficiaries.Model;
using TopupBeneficiaries.Services;

namespace TopupBeneficiaries.Test.Fixtures
{
	public class TopupValidatorFixture : IDisposable
	{
        public TopupValidatior topupValidatior;
        public List<TopUpTransaction> TransactionList = new List<TopUpTransaction>();
        public TopUpBeneficiary TopUpBeneficiary = new TopUpBeneficiary() { Id = 1L, PhoneNumber = "1234567890" };
        public TopUpOption TopUpOption = new TopUpOption() { Id = 7, TopUpAmount = 100m };

        public TopupValidatorFixture()
		{
            var loggerMock = new Mock<ILogger<TopupValidatior>>();
            var limitsAndChargesMock = new Mock<ILimitsAndCharges>();

            limitsAndChargesMock.Setup(m => m.TransactionCharges)
                .Returns(1);
            limitsAndChargesMock.Setup(m => m.TotalMonthlyLimit)
                .Returns(500);
            limitsAndChargesMock.Setup(m => m.VerifiedUserMonthlyLimit)
                .Returns(200);
            limitsAndChargesMock.Setup(m => m.UnVerifiedUserMonthlyLimit)
                .Returns(200);

            topupValidatior = new TopupValidatior(loggerMock.Object, limitsAndChargesMock.Object);
        }

        public void Dispose()
        {
            //Nothing to do for now
        }
    }
}

