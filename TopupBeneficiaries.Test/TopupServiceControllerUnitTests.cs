using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TopupBeneficiaries.Controllers;
using TopupBeneficiaries.Entities;
using TopupBeneficiaries.Model;
using TopupBeneficiaries.Repositories;
using TopupBeneficiaries.Services;

namespace TopupBeneficiaries.Test
{
    public class TopupServiceControllerUnitTests
    {
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<ILogger<TopupServiceController>> loggerMock;
        private Mock<ITopUpRepository> topupRepositoryMock;
        private Mock<ITopupValidator> topupValidatorMock;
        private Mock<ILimitsAndCharges> limitsAndChargesMock;
        private Mock<IUserFinanceService> userFinanceServiceMock;
        private TopupServiceController topupController;

        List<TopUpOption> topUpOptions = new List<TopUpOption>(
            new TopUpOption[]
            {
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
            });

        public TopupServiceControllerUnitTests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            loggerMock = new Mock<ILogger<TopupServiceController>>();
            topupRepositoryMock = new Mock<ITopUpRepository>();
            topupValidatorMock = new Mock<ITopupValidator>();
            limitsAndChargesMock = new Mock<ILimitsAndCharges>();
            userFinanceServiceMock = new Mock<IUserFinanceService>();

            topupRepositoryMock.Setup(m => m.GetAllTopUpOptionsAsync())
                .ReturnsAsync(topUpOptions);
            topupRepositoryMock.Setup(m => m.GetTopUpOptionByIdAsync(7))
                .ReturnsAsync(topUpOptions[6]);
        }

        [Fact]
        public async Task GetAllTopUpOptions_GetAction_VerifyTheListOfTopupOptions()
        {
            topupController = new TopupServiceController(loggerMock.Object,
                topupRepositoryMock.Object,
                userRepositoryMock.Object,
                topupValidatorMock.Object,
                limitsAndChargesMock.Object,
                userFinanceServiceMock.Object);

            var result = await topupController.GetAllTopUpOptions();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task TopupBeneficiary_SetAction_TopupOptionNotFound()
        {
            TopUpInputDto topUpInput = new TopUpInputDto() { TopUpBeneficiaryId = 1, TopUpOptionId = 8 };
            TopUpOption? topupOption = null;
            topupRepositoryMock.Setup(m => m.GetTopUpOptionByIdAsync(8))
                .ReturnsAsync(topupOption);

            topupController = new TopupServiceController(loggerMock.Object,
                topupRepositoryMock.Object,
                userRepositoryMock.Object,
                topupValidatorMock.Object,
                limitsAndChargesMock.Object,
                userFinanceServiceMock.Object);

            var result = await topupController.TopupBeneficiary(topUpInput);

            var errorCode = ((ErrorDto)((NotFoundObjectResult)result).Value).ErrorCode;

            Assert.Equal(21, errorCode);

        }

        [Fact]
        public async Task TopupBeneficiary_SetAction_TopupBeneficiaryNotFoundForTransaction()
        {
            TopUpInputDto topUpInput = new TopUpInputDto() { TopUpBeneficiaryId = 1L, TopUpOptionId = 7 };
            TopUpTransaction? topUpTransaction = null;

            topupRepositoryMock.Setup(m => m.GetTopUpOptionByIdAsync(8))
                .ReturnsAsync(topUpOptions[6]);
            topupRepositoryMock.Setup(m => m.BuildTransactionAsync(1L))
                .ReturnsAsync(topUpTransaction);

            topupController = new TopupServiceController(loggerMock.Object,
                topupRepositoryMock.Object,
                userRepositoryMock.Object,
                topupValidatorMock.Object,
                limitsAndChargesMock.Object,
                userFinanceServiceMock.Object);

            var result = await topupController.TopupBeneficiary(topUpInput);

            var errorCode = ((ErrorDto)((NotFoundObjectResult)result).Value).ErrorCode;

            Assert.Equal(22, errorCode);

        }

        [Fact]
        public async Task TopupBeneficiary_SetAction_UserHasInsufficiantBlalance()
        {
            TopUpInputDto topUpInput = new TopUpInputDto() { TopUpBeneficiaryId = 1L, TopUpOptionId = 7 };
            User user = new User
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
            TopUpTransaction topUpTransaction = new TopUpTransaction()
            {
                TopUpBeneficiaryId = 1L,
                TopUpBeneficiary = new TopUpBeneficiary()
                {

                    Id = 1L,
                    UserId = 1L,
                    NickName = "Bene 1",
                    PhoneNumber = "1234567890",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDateTime = DateTime.UtcNow,
                    LastUpdatedDateTime = DateTime.UtcNow
                },
                UserId = 1L,
                User = user
            };



            topupRepositoryMock.Setup(m => m.GetTopUpOptionByIdAsync(7))
                .ReturnsAsync(topUpOptions[6]);
            topupRepositoryMock.Setup(m => m.BuildTransactionAsync(1L))
                .ReturnsAsync(topUpTransaction);

            userFinanceServiceMock.Setup(m => m.GetBalanceAmount(It.IsAny<User>()))
                .Returns(50m);

            topupController = new TopupServiceController(loggerMock.Object,
                topupRepositoryMock.Object,
                userRepositoryMock.Object,
                topupValidatorMock.Object,
                limitsAndChargesMock.Object,
                userFinanceServiceMock.Object);

            var result = await topupController.TopupBeneficiary(topUpInput);

            var errorCode = ((ErrorDto)((ObjectResult)result).Value).ErrorCode;

            Assert.Equal(60, errorCode);
        }

        [Fact]
        public async Task TopupBeneficiary_SetAction_UserMonthlyTransactionLimitReached()
        {
            TopUpInputDto topUpInput = new TopUpInputDto() { TopUpBeneficiaryId = 1L, TopUpOptionId = 7 };
            User user = new User
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
            TopUpTransaction topUpTransaction = new TopUpTransaction()
            {
                TopUpBeneficiaryId = 1L,
                TopUpBeneficiary = new TopUpBeneficiary()
                {

                    Id = 1L,
                    UserId = 1L,
                    NickName = "Bene 1",
                    PhoneNumber = "1234567890",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDateTime = DateTime.UtcNow,
                    LastUpdatedDateTime = DateTime.UtcNow
                },
                UserId = 1L,
                User = user
            };



            topupRepositoryMock.Setup(m => m.GetTopUpOptionByIdAsync(7))
                .ReturnsAsync(topUpOptions[6]);
            topupRepositoryMock.Setup(m => m.BuildTransactionAsync(1L))
                .ReturnsAsync(topUpTransaction);

            userFinanceServiceMock.Setup(m => m.GetBalanceAmount(It.IsAny<User>()))
                .Returns(500m);

            userRepositoryMock.Setup(m => m.GetUserTransactionsByDateRangeAsync(
                1L,
                DateTime.UtcNow.AddMonths(-1),
                DateTime.UtcNow))
                .ReturnsAsync(new List<TopUpTransaction>());

            topupValidatorMock.Setup(m => m.ValidateUserTopupLimit(It.IsAny<User>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(false);


            topupController = new TopupServiceController(loggerMock.Object,
                topupRepositoryMock.Object,
                userRepositoryMock.Object,
                topupValidatorMock.Object,
                limitsAndChargesMock.Object,
                userFinanceServiceMock.Object);

            var result = await topupController.TopupBeneficiary(topUpInput);

            var errorCode = ((ErrorDto)((ObjectResult)result).Value).ErrorCode;

            Assert.Equal(51, errorCode);

        }


        [Fact]
        public async Task TopupBeneficiary_SetAction_VerifiedUserMonthlyTransactionLimitReached()
        {
            TopUpInputDto topUpInput = new TopUpInputDto() { TopUpBeneficiaryId = 1L, TopUpOptionId = 7 };
            User user = new User
            {
                Id = 1L,
                UserName = "Test User 1",
                PhoneNumber = "7863256215",
                BalanceAmount = 20000,
                IsVerifiedUser = true,
                IsDeleted = false,
                CreatedDateTime = DateTime.UtcNow,
                LastUpdatedDateTime = DateTime.UtcNow
            };
            TopUpTransaction topUpTransaction = new TopUpTransaction()
            {
                TopUpBeneficiaryId = 1L,
                TopUpBeneficiary = new TopUpBeneficiary()
                {

                    Id = 1L,
                    UserId = 1L,
                    NickName = "Bene 1",
                    PhoneNumber = "1234567890",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDateTime = DateTime.UtcNow,
                    LastUpdatedDateTime = DateTime.UtcNow
                },
                UserId = 1L,
                User = user
            };



            topupRepositoryMock.Setup(m => m.GetTopUpOptionByIdAsync(7))
                .ReturnsAsync(topUpOptions[6]);
            topupRepositoryMock.Setup(m => m.BuildTransactionAsync(1L))
                .ReturnsAsync(topUpTransaction);

            userFinanceServiceMock.Setup(m => m.GetBalanceAmount(It.IsAny<User>()))
                .Returns(500m);

            userRepositoryMock.Setup(m => m.GetUserTransactionsByDateRangeAsync(
                1L,
                DateTime.UtcNow.AddMonths(-1),
                DateTime.UtcNow))
                .ReturnsAsync(new List<TopUpTransaction>());

            topupValidatorMock.Setup(m => m.ValidateUserTopupLimit(It.IsAny<User>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);
            topupValidatorMock.Setup(m => m.ValidateVerifiedUserTopup(It.IsAny<User>(),
                It.IsAny<TopUpBeneficiary>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(false);



            topupController = new TopupServiceController(loggerMock.Object,
                topupRepositoryMock.Object,
                userRepositoryMock.Object,
                topupValidatorMock.Object,
                limitsAndChargesMock.Object,
                userFinanceServiceMock.Object);

            var result = await topupController.TopupBeneficiary(topUpInput);

            var errorCode = ((ErrorDto)((ObjectResult)result).Value).ErrorCode;

            Assert.Equal(50, errorCode);

        }

        [Fact]
        public async Task TopupBeneficiary_SetAction_UnVerifiedUserMonthlyTransactionLimitReached()
        {
            TopUpInputDto topUpInput = new TopUpInputDto() { TopUpBeneficiaryId = 1L, TopUpOptionId = 7 };
            User user = new User
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
            TopUpTransaction topUpTransaction = new TopUpTransaction()
            {
                TopUpBeneficiaryId = 1L,
                TopUpBeneficiary = new TopUpBeneficiary()
                {

                    Id = 1L,
                    UserId = 1L,
                    NickName = "Bene 1",
                    PhoneNumber = "1234567890",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDateTime = DateTime.UtcNow,
                    LastUpdatedDateTime = DateTime.UtcNow
                },
                UserId = 1L,
                User = user
            };



            topupRepositoryMock.Setup(m => m.GetTopUpOptionByIdAsync(7))
                .ReturnsAsync(topUpOptions[6]);
            topupRepositoryMock.Setup(m => m.BuildTransactionAsync(1L))
                .ReturnsAsync(topUpTransaction);

            userFinanceServiceMock.Setup(m => m.GetBalanceAmount(It.IsAny<User>()))
                .Returns(500m);

            userRepositoryMock.Setup(m => m.GetUserTransactionsByDateRangeAsync(
                1L,
                DateTime.UtcNow.AddMonths(-1),
                DateTime.UtcNow))
                .ReturnsAsync(new List<TopUpTransaction>());

            topupValidatorMock.Setup(m => m.ValidateUserTopupLimit(It.IsAny<User>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);
            topupValidatorMock.Setup(m => m.ValidateVerifiedUserTopup(It.IsAny<User>(),
                It.IsAny<TopUpBeneficiary>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);

            topupValidatorMock.Setup(m => m.ValidateUnVerifiedUserTopup(It.IsAny<User>(),
                It.IsAny<TopUpBeneficiary>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(false);


            topupController = new TopupServiceController(loggerMock.Object,
                topupRepositoryMock.Object,
                userRepositoryMock.Object,
                topupValidatorMock.Object,
                limitsAndChargesMock.Object,
                userFinanceServiceMock.Object);

            var result = await topupController.TopupBeneficiary(topUpInput);

            var errorCode = ((ErrorDto)((ObjectResult)result).Value).ErrorCode;

            Assert.Equal(50, errorCode);

        }

        [Fact]
        public async Task TopupBeneficiary_SetAction_UnableToDebitAmountFromUser()
        {
            TopUpInputDto topUpInput = new TopUpInputDto() { TopUpBeneficiaryId = 1L, TopUpOptionId = 7 };
            User user = new User
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
            TopUpTransaction topUpTransaction = new TopUpTransaction()
            {
                TopUpBeneficiaryId = 1L,
                TopUpBeneficiary = new TopUpBeneficiary()
                {

                    Id = 1L,
                    UserId = 1L,
                    NickName = "Bene 1",
                    PhoneNumber = "1234567890",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDateTime = DateTime.UtcNow,
                    LastUpdatedDateTime = DateTime.UtcNow
                },
                UserId = 1L,
                User = user
            };



            topupRepositoryMock.Setup(m => m.GetTopUpOptionByIdAsync(7))
                .ReturnsAsync(topUpOptions[6]);
            topupRepositoryMock.Setup(m => m.BuildTransactionAsync(1L))
                .ReturnsAsync(topUpTransaction);

            userFinanceServiceMock.Setup(m => m.GetBalanceAmount(It.IsAny<User>()))
                .Returns(500m);

            userRepositoryMock.Setup(m => m.GetUserTransactionsByDateRangeAsync(
                1L,
                DateTime.UtcNow.AddMonths(-1),
                DateTime.UtcNow))
                .ReturnsAsync(new List<TopUpTransaction>());

            topupValidatorMock.Setup(m => m.ValidateUserTopupLimit(It.IsAny<User>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);
            topupValidatorMock.Setup(m => m.ValidateVerifiedUserTopup(It.IsAny<User>(),
                It.IsAny<TopUpBeneficiary>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);

            topupValidatorMock.Setup(m => m.ValidateUnVerifiedUserTopup(It.IsAny<User>(),
                It.IsAny<TopUpBeneficiary>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);

            userFinanceServiceMock.Setup(m => m.DebitAmount(It.IsAny<User>(), 100))
                .Returns(false);

            topupController = new TopupServiceController(loggerMock.Object,
                topupRepositoryMock.Object,
                userRepositoryMock.Object,
                topupValidatorMock.Object,
                limitsAndChargesMock.Object,
                userFinanceServiceMock.Object);

            var result = await topupController.TopupBeneficiary(topUpInput);

            var errorCode = ((ErrorDto)((ObjectResult)result).Value).ErrorCode;

            Assert.Equal(63, errorCode);

        }

        [Fact]
        public async Task TopupBeneficiary_SetAction_UnableToSaveTransactionToDatabase()
        {
            TopUpInputDto topUpInput = new TopUpInputDto() { TopUpBeneficiaryId = 1L, TopUpOptionId = 7 };
            User user = new User
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
            TopUpTransaction topUpTransaction = new TopUpTransaction()
            {
                TopUpBeneficiaryId = 1L,
                TopUpBeneficiary = new TopUpBeneficiary()
                {

                    Id = 1L,
                    UserId = 1L,
                    NickName = "Bene 1",
                    PhoneNumber = "1234567890",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDateTime = DateTime.UtcNow,
                    LastUpdatedDateTime = DateTime.UtcNow
                },
                UserId = 1L,
                User = user
            };



            topupRepositoryMock.Setup(m => m.GetTopUpOptionByIdAsync(7))
                .ReturnsAsync(topUpOptions[6]);
            topupRepositoryMock.Setup(m => m.BuildTransactionAsync(1L))
                .ReturnsAsync(topUpTransaction);

            userFinanceServiceMock.Setup(m => m.GetBalanceAmount(It.IsAny<User>()))
                .Returns(500m);

            userRepositoryMock.Setup(m => m.GetUserTransactionsByDateRangeAsync(
                1L,
                DateTime.UtcNow.AddMonths(-1),
                DateTime.UtcNow))
                .ReturnsAsync(new List<TopUpTransaction>());

            topupValidatorMock.Setup(m => m.ValidateUserTopupLimit(It.IsAny<User>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);
            topupValidatorMock.Setup(m => m.ValidateVerifiedUserTopup(It.IsAny<User>(),
                It.IsAny<TopUpBeneficiary>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);

            topupValidatorMock.Setup(m => m.ValidateUnVerifiedUserTopup(It.IsAny<User>(),
                It.IsAny<TopUpBeneficiary>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);

            userFinanceServiceMock.Setup(m => m.DebitAmount(It.IsAny<User>(), 100))
                .Returns(true);

            topupRepositoryMock.Setup(m => m.SaveTopUpTransactionAsync(It.IsAny<TopUpTransaction>()))
                .ReturnsAsync(false);

            topupController = new TopupServiceController(loggerMock.Object,
                topupRepositoryMock.Object,
                userRepositoryMock.Object,
                topupValidatorMock.Object,
                limitsAndChargesMock.Object,
                userFinanceServiceMock.Object);

            var result = await topupController.TopupBeneficiary(topUpInput);

            var errorCode = ((ErrorDto)((ObjectResult)result).Value).ErrorCode;

            Assert.Equal(65, errorCode);

        }

        [Fact]
        public async Task TopupBeneficiary_SetAction_TopupSuccess()
        {
            TopUpInputDto topUpInput = new TopUpInputDto() { TopUpBeneficiaryId = 1L, TopUpOptionId = 7 };
            User user = new User
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
            TopUpTransaction topUpTransaction = new TopUpTransaction()
            {
                TopUpBeneficiaryId = 1L,
                TopUpBeneficiary = new TopUpBeneficiary()
                {

                    Id = 1L,
                    UserId = 1L,
                    NickName = "Bene 1",
                    PhoneNumber = "1234567890",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDateTime = DateTime.UtcNow,
                    LastUpdatedDateTime = DateTime.UtcNow
                },
                UserId = 1L,
                User = user
            };



            topupRepositoryMock.Setup(m => m.GetTopUpOptionByIdAsync(7))
                .ReturnsAsync(topUpOptions[6]);
            topupRepositoryMock.Setup(m => m.BuildTransactionAsync(1L))
                .ReturnsAsync(topUpTransaction);

            userFinanceServiceMock.Setup(m => m.GetBalanceAmount(It.IsAny<User>()))
                .Returns(500m);

            userRepositoryMock.Setup(m => m.GetUserTransactionsByDateRangeAsync(
                1L,
                DateTime.UtcNow.AddMonths(-1),
                DateTime.UtcNow))
                .ReturnsAsync(new List<TopUpTransaction>());

            topupValidatorMock.Setup(m => m.ValidateUserTopupLimit(It.IsAny<User>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);
            topupValidatorMock.Setup(m => m.ValidateVerifiedUserTopup(It.IsAny<User>(),
                It.IsAny<TopUpBeneficiary>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);

            topupValidatorMock.Setup(m => m.ValidateUnVerifiedUserTopup(It.IsAny<User>(),
                It.IsAny<TopUpBeneficiary>(),
                It.IsAny<TopUpOption>(),
                It.IsAny<List<TopUpTransaction>>()))
                .Returns(true);

            userFinanceServiceMock.Setup(m => m.DebitAmount(It.IsAny<User>(), 100))
                .Returns(true);

            topupRepositoryMock.Setup(m => m.SaveTopUpTransactionAsync(It.IsAny<TopUpTransaction>()))
                .ReturnsAsync(true);

            topupController = new TopupServiceController(loggerMock.Object,
                topupRepositoryMock.Object,
                userRepositoryMock.Object,
                topupValidatorMock.Object,
                limitsAndChargesMock.Object,
                userFinanceServiceMock.Object);

            var result = await topupController.TopupBeneficiary(topUpInput);

            Assert.IsType<NoContentResult>(result);

        }
    }
}

