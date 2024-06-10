using System;
using Moq;
using TopupBeneficiaries.Entities;
using TopupBeneficiaries.Controllers;
using TopupBeneficiaries.Model;
using TopupBeneficiaries.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace TopupBeneficiaries.Test
{
	public class UserServiceControllerUnitTests
	{
		private Mock<IUserRepository> userRepositoryMock;
		private Mock<ILogger<UserServiceController>> loggerMock;
		private User expectedUserAndBeneficiaries;

		AddBeneficiaryDto inputBeneficiary = new AddBeneficiaryDto()
		{
			UserId = 1L,
			NickName = "Bene 6",
			PhoneNumber = "9876543200"
		};


		List<TopUpBeneficiary> expectedBeneficiaries = new List<TopUpBeneficiary>(
			new TopUpBeneficiary[]
			{
				new TopUpBeneficiary()
				{
					Id = 1L, UserId = 1L, NickName = "Bene 1", PhoneNumber = "1234567890", IsActive = true, IsDeleted = false,
					CreatedDateTime = DateTime.UtcNow, LastUpdatedDateTime = DateTime.UtcNow }
				});
        public UserServiceControllerUnitTests()
		{
			userRepositoryMock = new Mock<IUserRepository>();
			loggerMock = new Mock<ILogger<UserServiceController>>();

			
			expectedUserAndBeneficiaries = new User {
				Id = 1L,
				UserName = "Test User 1",
				PhoneNumber = "7863256215",
				BalanceAmount = 20000,
				IsVerifiedUser = false,
				IsDeleted = false,
				CreatedDateTime = DateTime.UtcNow,
				LastUpdatedDateTime = DateTime.UtcNow,
				TopUpBeneficiaries = expectedBeneficiaries
			};



            userRepositoryMock.Setup(m => m.GetActiveBeneficiariesAsync(1L))
				.ReturnsAsync(expectedBeneficiaries);
			userRepositoryMock.Setup(m => m.GetUserAndBeneficiariesAsync(1L))
				.ReturnsAsync(expectedUserAndBeneficiaries);
          
        }

		[Fact]
		public async Task GetActiveBeneficiaries_GetAction_MustReturnBeneficiary()
		{
			UserServiceController controller = new UserServiceController(loggerMock.Object, userRepositoryMock.Object);
			var result = await controller.GetActiveBeneficiaries(1L);

			var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
			Assert.IsType<OkObjectResult>(actionResult);
		}

		[Fact]
		public async Task AddBeneficiaries_SetAction_ForSuccess()
		{
            userRepositoryMock.Setup(m => m.AddBeneficiary(It.IsAny<TopUpBeneficiary>(), It.IsAny<User>()))
              .Returns(true);
            UserServiceController controller = new UserServiceController(loggerMock.Object, userRepositoryMock.Object);
			var result = await controller.AddBeneficiary(inputBeneficiary);

			Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AddBeneficiary_SetAction_ForFailure()
        {
            userRepositoryMock.Setup(m => m.AddBeneficiary(It.IsAny<TopUpBeneficiary>(), It.IsAny<User>()))
              .Returns(false);
            UserServiceController controller = new UserServiceController(loggerMock.Object, userRepositoryMock.Object);
            var result = await controller.AddBeneficiary(inputBeneficiary);

			var errorCode = ((ErrorDto)((ObjectResult)result).Value).ErrorCode;

			Assert.Equal(10, errorCode);
        }

        [Fact]
        public async Task AddBeneficiary_SetAction_UserNotFoundError()
        {
            User? returnObject = null;
            userRepositoryMock.Setup(m => m.GetUserAndBeneficiariesAsync(1L))
                .ReturnsAsync(returnObject);
            UserServiceController controller = new UserServiceController(loggerMock.Object, userRepositoryMock.Object);
            var result = await controller.AddBeneficiary(inputBeneficiary);

            Assert.IsType<NotFoundObjectResult>(result);

        }

        [Fact]
        public async Task AddBeneficiary_SetAction_ErrorPhoneNumberAlreadyExists()
        {
            TopUpBeneficiary beneficiaryToAdd = new TopUpBeneficiary()
            {
                Id = 10L,
                UserId = 1L,
                NickName = "Bene 6",
                PhoneNumber = "1234567890",
                IsActive = true,
                IsDeleted = false,
                CreatedDateTime = DateTime.UtcNow,
                LastUpdatedDateTime = DateTime.UtcNow
            };

            AddBeneficiaryDto beneficiaryDto = new AddBeneficiaryDto()
            {
                UserId = 1L,
                NickName = "Bene 6",
                PhoneNumber = "1234567890"
            };


            userRepositoryMock.Setup(m => m.AddBeneficiary(beneficiaryToAdd,  It.IsAny<User>()))
              .Returns(false);
            UserServiceController controller = new UserServiceController(loggerMock.Object, userRepositoryMock.Object);
            var result = await controller.AddBeneficiary(beneficiaryDto);

            var errorCode = ((ErrorDto)((BadRequestObjectResult)result).Value).ErrorCode;

            Assert.Equal(2, errorCode);
        }

        [Fact]
        public async Task AddBeneficiary_SetAction_FailForMoreThan5ActiveBeneficiaries()
        {
            var beneficiaries = new List<TopUpBeneficiary>(
            new TopUpBeneficiary[]
            {
                        new TopUpBeneficiary()
                        {
                            Id = 1L, UserId = 1L, NickName = "Bene 1", PhoneNumber = "1234567890", IsActive = true, IsDeleted = false,
                            CreatedDateTime = DateTime.UtcNow, LastUpdatedDateTime = DateTime.UtcNow
                        },
                        new TopUpBeneficiary()
                        {
                            Id = 2L,
                            UserId = 1L,
                            NickName = "Bene 2",
                            PhoneNumber = "1234567891",
                            IsActive = true,
                            IsDeleted = false,
                            CreatedDateTime = DateTime.UtcNow,
                            LastUpdatedDateTime = DateTime.UtcNow
                        },
                        new TopUpBeneficiary()
                        {
                            Id = 3L,
                            UserId = 1L,
                            NickName = "Bene 3",
                            PhoneNumber = "1234567892",
                            IsActive = true,
                            IsDeleted = false,
                            CreatedDateTime = DateTime.UtcNow,
                            LastUpdatedDateTime = DateTime.UtcNow
                        },
                        new TopUpBeneficiary()
                        {
                            Id = 4L,
                            UserId = 1L,
                            NickName = "Bene 4",
                            PhoneNumber = "1234567893",
                            IsActive = true,
                            IsDeleted = false,
                            CreatedDateTime = DateTime.UtcNow,
                            LastUpdatedDateTime = DateTime.UtcNow
                        },
                        new TopUpBeneficiary()
                        {
                            Id = 5L,
                            UserId = 1L,
                            NickName = "Bene 5",
                            PhoneNumber = "1234567894",
                            IsActive = true,
                            IsDeleted = false,
                            CreatedDateTime = DateTime.UtcNow,
                            LastUpdatedDateTime = DateTime.UtcNow
                        }
            });


            var expectedUser = new User
            {
                Id = 1L,
                UserName = "Test User 1",
                PhoneNumber = "7863256215",
                BalanceAmount = 20000,
                IsVerifiedUser = false,
                IsDeleted = false,
                CreatedDateTime = DateTime.UtcNow,
                LastUpdatedDateTime = DateTime.UtcNow,
                TopUpBeneficiaries = beneficiaries
            };

            userRepositoryMock.Setup(m => m.AddBeneficiary(It.IsAny<TopUpBeneficiary>(), It.IsAny<User>()))
				.Returns(true);

            userRepositoryMock.Setup(m => m.GetUserAndBeneficiariesAsync(1L))
				.ReturnsAsync(expectedUser);
            UserServiceController controller = new UserServiceController(loggerMock.Object, userRepositoryMock.Object);
            var result = await controller.AddBeneficiary(inputBeneficiary);

            var errorCode = ((ErrorDto)((ObjectResult)result).Value).ErrorCode;
            Assert.Equal(5, errorCode);
        }
    }
}

