using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TopupBeneficiaries.DBContext;
using TopupBeneficiaries.Entities;
using TopupBeneficiaries.Repositories;

namespace TopupBeneficiaries.Test.Fixtures
{
	[Collection("DataRepositoryCollection")]
	public class DataRepositoryFixture : IDisposable
	{
		public IUserRepository userRepositoryTest { get; }
		public ITopUpRepository topUpRepositoryTest { get; }
		private SqliteConnection connection;

        public DataRepositoryFixture()
		{
			connection = new SqliteConnection("Data Source=:memory:");
			connection.Open();
			var optionsBuilder = new DbContextOptionsBuilder<TopupBeneficiaryContext>().UseSqlite(connection);
			var dbContext = new TopupBeneficiaryContext(optionsBuilder.Options);
			dbContext.Database.Migrate();

			//Add few transactions
			dbContext.TopUpTransactions.Add(new TopUpTransaction()
			{
				TopUpBeneficiary = dbContext.TopUpBeneficiaries.First<TopUpBeneficiary>(b => b.Id==1L),
				TopUpChargeAmount = 1,
				TopUpOption = dbContext.topUpOptions.First<TopUpOption>(o => o.Id == 7),
				User = dbContext.Users.First<User>(u => u.Id == 1L),
				TopupDateTime = DateTime.UtcNow
			});

            dbContext.TopUpTransactions.Add(new TopUpTransaction()
            {
                TopUpBeneficiary = dbContext.TopUpBeneficiaries.First<TopUpBeneficiary>(b => b.Id == 2L),
                TopUpChargeAmount = 1,
                TopUpOption = dbContext.topUpOptions.First<TopUpOption>(o => o.Id == 6),
                User = dbContext.Users.First<User>(u => u.Id == 1L),
                TopupDateTime = DateTime.UtcNow
            });
			var cnt = dbContext.SaveChanges();

            var topUpRepoLoggerMock = new Mock<ILogger<TopUpRepository>>();
			topUpRepositoryTest = new TopUpRepository(topUpRepoLoggerMock.Object, dbContext);

			var userRepoLoggerMock = new Mock<ILogger<UserRepository>>();
			userRepositoryTest = new UserRepository(userRepoLoggerMock.Object, dbContext);

		}

        public void Dispose()
        {
			if (connection != null && connection.State != System.Data.ConnectionState.Closed)
			{
				connection.Close();
				connection.Dispose();
			}
           
        }
    }
}

