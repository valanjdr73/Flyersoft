using System;
using TopupBeneficiaries.Entities;
using TopupBeneficiaries.Test.Fixtures;

namespace TopupBeneficiaries.Test
{
	[Collection("DataRepositoryCollection")]
	public class UserRepositoryUnitTests
	{
		private readonly DataRepositoryFixture _fixture;
		public UserRepositoryUnitTests( DataRepositoryFixture repositoryFixture)
		{
			_fixture = repositoryFixture;
		}

		[Fact]
		public async Task GetActiveBeneficiariesAsync_GetActiveBeneficiaries_GetListOfActiveBeneficiaries()
		{
			//Seeded 3 beneficiaries
			var topupBeneficiaries = await _fixture.userRepositoryTest.GetActiveBeneficiariesAsync(userId: 1L);
			Assert.True(topupBeneficiaries.Count() == 3);
			Assert.Contains(topupBeneficiaries, beneficiary => beneficiary.NickName == "Bene 3");
		}

		[Fact]
		public async Task GetUserAndBeneficiariesAsync_GetUserAndBeneficiaries_GetUserDetailAndBeneficiaryDetail()
		{
			var user = await _fixture.userRepositoryTest.GetUserAndBeneficiariesAsync(userId: 1L);
			Assert.True(user?.Id == 1L);
            Assert.Contains(user?.TopUpBeneficiaries??new List<TopUpBeneficiary>(), beneficiary => beneficiary.NickName == "Bene 3");
        }

		[Fact]
		public async Task GetBeneficiaryDetailsAsync_GetBeneficiaryDetail_GetBeneficiarDetailIncludingUserDetail()
		{
			var beneficiary = await _fixture.userRepositoryTest.GetBeneficiaryDetailsAsync(3L);
			Assert.True(beneficiary?.NickName == "Bene 3");
			Assert.True(beneficiary?.User.UserName == "Test User 1");
		}

		[Fact]
		public async Task GetActiveBeneficiaryCountAsync_GetActiveBeneficiaryCount_MatchCount()
		{
			var count = await _fixture.userRepositoryTest.GetActiveBeneficiaryCountAsync(1L);
			Assert.True(count == 3);
		}

		[Fact]
		public async Task AddBeneficiary_AddBeneficiary_VerifyBeneficiaryAdded()
		{
            var user = await _fixture.userRepositoryTest.GetUserAndBeneficiariesAsync(userId: 2L);
            Assert.True(user?.Id == 2L);
			bool added = _fixture.userRepositoryTest.AddBeneficiary(
				beneficiary: new TopUpBeneficiary()
                {
                    NickName = "Bene 4",
                    PhoneNumber = "1234567880",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDateTime = DateTime.UtcNow,
                    LastUpdatedDateTime = DateTime.UtcNow
                },
				user: user);
			Assert.True(added);
        }

		[Fact]
		public async Task GetUserTransactionsByDateRangeAsync_GetUserTransactions_VerifyByListOfTransactions()
		{
			var transactionList = await _fixture.userRepositoryTest.GetUserTransactionsByDateRangeAsync(1L, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow);
			Assert.True(transactionList.Count() > 0);
		}
    }
}

