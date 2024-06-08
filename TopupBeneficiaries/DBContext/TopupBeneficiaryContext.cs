using System;
using Microsoft.EntityFrameworkCore;
using TopupBeneficiaries.Entities;

namespace TopupBeneficiaries.DBContext
{
	public class TopupBeneficiaryContext : DbContext
	{
		public DbSet<TopUpOption> topUpOptions { get; set; }
		public DbSet<User> Users { get; set; }
        public DbSet<TopUpBeneficiary> TopUpBeneficiaries { get; set; }
		public DbSet<TopUpTransaction> TopUpTransactions { get; set; }

		public TopupBeneficiaryContext(DbContextOptions<TopupBeneficiaryContext> options)
			: base(options)
		{
            
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasMany(a => a.TopUpBeneficiaries).WithOne(b => b.User).HasForeignKey(c => c.UserId);
            modelBuilder.Entity<TopUpTransaction>().HasOne(a => a.User).WithMany(b => b.TopUpTransactions).HasForeignKey(c => c.UserId);
            modelBuilder.Entity<TopUpTransaction>().HasOne(a => a.TopUpBeneficiary).WithMany(b => b.TopUpTransactions).HasForeignKey(c => c.TopUpBeneficiaryId);
            modelBuilder.Entity<TopUpTransaction>().HasOne(a => a.TopUpOption).WithMany(b => b.TopUpTransactions).HasForeignKey(c => c.TopUpOptionId);


            //seed data for first time use
			modelBuilder.Entity<TopUpOption>()
				.HasData(
					new TopUpOption()
					{
						Id = 1,
						Name = "5",
						TopUpAmount = 5,
						IsDeleted = false,
						CreatedDateTime = DateTime.UtcNow,
						LastUpdateDateTime = DateTime.UtcNow
					},
                    new TopUpOption()
                    {
                        Id = 2,
                        Name = "10",
                        TopUpAmount = 10,
                        IsDeleted = false,
                        CreatedDateTime = DateTime.UtcNow,
                        LastUpdateDateTime = DateTime.UtcNow
                    },
                    new TopUpOption()
                    {
                        Id = 3,
                        Name = "20",
                        TopUpAmount = 20,
                        IsDeleted = false,
                        CreatedDateTime = DateTime.UtcNow,
                        LastUpdateDateTime = DateTime.UtcNow
                    },
                    new TopUpOption()
                    {
                        Id = 4,
                        Name = "30",
                        TopUpAmount = 30,
                        IsDeleted = false,
                        CreatedDateTime = DateTime.UtcNow,
                        LastUpdateDateTime = DateTime.UtcNow
                    },
                    new TopUpOption()
                    {
                        Id = 5,
                        Name = "50",
                        TopUpAmount = 50,
                        IsDeleted = false,
                        CreatedDateTime = DateTime.UtcNow,
                        LastUpdateDateTime = DateTime.UtcNow
                    },
                    new TopUpOption()
                    {
                        Id = 6,
                        Name = "75",
                        TopUpAmount = 75,
                        IsDeleted = false,
                        CreatedDateTime = DateTime.UtcNow,
                        LastUpdateDateTime = DateTime.UtcNow
                    },
                    new TopUpOption()
                    {
                        Id = 7,
                        Name = "100",
                        TopUpAmount = 100,
                        IsDeleted = false,
                        CreatedDateTime = DateTime.UtcNow,
                        LastUpdateDateTime = DateTime.UtcNow
                    }
                );
            User User1 = new User()
            {
                Id = 1L,
                UserName = "Test User 1",
                PhoneNumber = "7863256215",
                BalanceAmount = 20000,
                IsVerifiedUser = false,
                IsDeleted = false,
                CreatedDateTime = DateTime.UtcNow,
                LastUpdatedDateTime = DateTime.UtcNow
            };
            User User2 = new User()
            {
                Id = 2L,
                UserName = "Test User 2",
                PhoneNumber = "9863256215",
                BalanceAmount = 4000,
                IsVerifiedUser = true,
                IsDeleted = false,
                CreatedDateTime = DateTime.UtcNow,
                LastUpdatedDateTime = DateTime.UtcNow
            };
            modelBuilder.Entity<User>()
                .HasData(User1, User2);
            modelBuilder.Entity<TopUpBeneficiary>()
                .HasData(
                            new {
                                Id = 1L,
                                UserId = 1L,  NickName = "Bene 1", PhoneNumber = "1234567890", IsActive = true, IsDeleted = false, CreatedDateTime = DateTime.UtcNow, LastUpdatedDateTime = DateTime.UtcNow },
                            new {
                                Id = 2L,
                                UserId = 1L,  NickName = "Bene 2", PhoneNumber = "1234567891", IsActive = true, IsDeleted = false, CreatedDateTime = DateTime.UtcNow, LastUpdatedDateTime = DateTime.UtcNow },
                            new {
                                Id = 3L,
                                UserId = 1L,  NickName = "Bene 3", PhoneNumber = "1234567892", IsActive = true, IsDeleted = false, CreatedDateTime = DateTime.UtcNow, LastUpdatedDateTime = DateTime.UtcNow },
                            new {
                                Id = 4L,
                                UserId = 2L,  NickName = "Bene 1", PhoneNumber = "9876543210", IsActive = true, IsDeleted = false, CreatedDateTime = DateTime.UtcNow, LastUpdatedDateTime = DateTime.UtcNow },
                            new {
                                Id = 5L,
                                UserId = 2L,  NickName = "Bene 2", PhoneNumber = "9876543211", IsActive = true, IsDeleted = false, CreatedDateTime = DateTime.UtcNow, LastUpdatedDateTime = DateTime.UtcNow },
                            new {
                                Id = 6L,
                                UserId = 2L,  NickName = "Bene 3", PhoneNumber = "9876543212", IsActive = true, IsDeleted = false, CreatedDateTime = DateTime.UtcNow, LastUpdatedDateTime = DateTime.UtcNow }

                );

            base.OnModelCreating(modelBuilder);
        }
    }
}

