using System;
using TopupBeneficiaries.Entities;
using TopupBeneficiaries.Test.Fixtures;

namespace TopupBeneficiaries.Test
{
    [Collection("DataRepositoryCollection")]
    public class TopUpRepositoryUnitTests : IDisposable
	{
        private readonly DataRepositoryFixture _fixture;
        public TopUpRepositoryUnitTests(DataRepositoryFixture repositoryFixture)
        {
            _fixture = repositoryFixture;
        }

        [Fact]
        public async Task GetAllTopUpOptionsAsync_GetTopUpOptions_VerifyList()
        {
            var topUpOptions = await _fixture.topUpRepositoryTest.GetAllTopUpOptionsAsync();
            Assert.Contains(topUpOptions, options => options.TopUpAmount == 50.0m);
        }

        [Fact]
        public async Task GetTopUpOptionByIdAsync_GetTopUpOption_ValueMustBe100ForOption7()
        {
            var topUpOption = await _fixture.topUpRepositoryTest.GetTopUpOptionByIdAsync(7);
            Assert.True(topUpOption?.TopUpAmount == 100.0m);
        }

        [Fact]
        public async Task BuildTransactionAsync_BuildATransaction_ConstructByAddingUser1AndBeneficiary3()
        {
            var transaction = await _fixture.topUpRepositoryTest.BuildTransactionAsync(3L);
            Assert.True(transaction?.TopUpBeneficiary.Id == 3L && transaction?.User.Id == 1L);
        }

        [Fact]
        public async Task SaveTopUpTransactionAsync_SaveAConstructedTransaction_MustReturnTrue()
        {
            var transaction = await _fixture.topUpRepositoryTest.BuildTransactionAsync(3L);
            Assert.True(transaction?.TopUpBeneficiary.Id == 3L && transaction?.User.Id == 1L);
            transaction.TopUpChargeAmount = 1;
            transaction.TopupDateTime = DateTime.UtcNow;
            var success = await _fixture.topUpRepositoryTest.SaveTopUpTransactionAsync(transaction);
            Assert.True(success);
        }

        public void Dispose()
        {
           //Nothing to do for now
        }
    }
}

