using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TopupBeneficiaries.Model;
using TopupBeneficiaries.Repositories;
using TopupBeneficiaries.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TopupBeneficiaries.Controllers
{
    [ApiController]
    [Route("api/TopupService")]
    public class TopupServiceController : ControllerBase
    {
        private readonly ITopUpRepository _topupRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TopupServiceController> _logger;
        private readonly ITopupValidator topupValidator;
        private readonly ILimitsAndCharges limitSettings;
        private readonly IUserFinanceService financeService;

        public TopupServiceController(ILogger<TopupServiceController> logger,
            ITopUpRepository topUpRepository,
            IUserRepository userRepository,
            ITopupValidator topupValidator,
            ILimitsAndCharges limitsAndCharges,
            IUserFinanceService userFinanceService)
        {
            _topupRepository = topUpRepository;
            _userRepository = userRepository;
            _logger = logger;
            this.topupValidator = topupValidator;
            limitSettings = limitsAndCharges;
            financeService = userFinanceService;
        }


        [HttpGet]
        [Route("GetTopUpOptions")]
        public async Task<IActionResult> GetAllTopUpOptions()
        {
            try
            {
                return Ok(await _topupRepository.GetAllTopUpOptionsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Getting Topup Options");
                return StatusCode(500, new ErrorDto { ErrorCode = 999, ErrorMessage = "Error getting topup options" });
            }
        }

        [HttpPost]
        [Route("Topup")]
        public async Task<IActionResult> TopupBeneficiary(TopUpInputDto topUpInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                //Don't see a need for Unit of Work pattern, only reads across repository

                var topUpOption = await _topupRepository.GetTopUpOptionByIdAsync(topUpInput.TopUpOptionId);
                if (topUpOption == null)
                {
                    return NotFound(new ErrorDto { ErrorCode = 21, ErrorMessage = "TopUp Option not found" });
                }

                var topUpTransaction = await _topupRepository.BuildTransactionAsync(topUpInput.TopUpBeneficiaryId);
                if (topUpTransaction == null)
                {
                    return NotFound(new ErrorDto { ErrorCode=22, ErrorMessage = "TopUp Beneficiary is not found" });
                }
                topUpTransaction.TopUpOption = topUpOption;
                topUpTransaction.TopUpOptionId = topUpOption.Id;
                DateTime endDate = DateTime.UtcNow;
                DateTime startDate = endDate.AddMonths(-1);

                //Call to find out whether the user has sufficiant balance from external webservice else quit
                try
                {
                    if (financeService.GetBalanceAmount(topUpTransaction.User) < topUpOption.TopUpAmount + limitSettings.TransactionCharges)
                    {
                        return StatusCode(500, new ErrorDto { ErrorCode = 60, ErrorMessage = "User has insufficiant balance for current topup" });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing financial transaction to get balance");
                    return StatusCode(500, new ErrorDto { ErrorCode = 65, ErrorMessage = "Error processing financial transaction for Get balance" });
                }

                var transactionList = await _userRepository.GetUserTransactionsByDateRangeAsync(topUpTransaction.UserId, startDate, endDate);
                bool validationPassed = false;
                if (topupValidator.ValidateUserTopupLimit(topUpTransaction.User, topUpOption, transactionList))
                {
                    if (topUpTransaction.User.IsVerifiedUser)
                    {
                        validationPassed = topupValidator.ValidateVerifiedUserTopup(topUpTransaction.User, topUpTransaction.TopUpBeneficiary, topUpOption, transactionList);
                    }
                    else
                    {
                        validationPassed = topupValidator.ValidateUnVerifiedUserTopup(topUpTransaction.User, topUpTransaction.TopUpBeneficiary, topUpOption, transactionList);
                    }
                    if (validationPassed)
                    {

                        topUpTransaction.TopupDateTime = DateTime.UtcNow;
                        topUpTransaction.TopUpChargeAmount = limitSettings.TransactionCharges;

                        //Call to debit topup amount from user balance (External web service)
                        try
                        {
                            if (!financeService.DebitAmount(topUpTransaction.User, topUpOption.TopUpAmount + limitSettings.TransactionCharges))
                            {
                                throw new ApplicationException("Error processing debit amount from user account");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing financial transaction");
                            return StatusCode(500, new ErrorDto { ErrorCode = 63, ErrorMessage = "Error processing debit amount from user account" });
                        }

                        await _topupRepository.SaveTopUpTransactionAsync(topUpTransaction);
                        return NoContent();
                    }
                    else
                    {
                        return StatusCode(500, new ErrorDto { ErrorCode=50, ErrorMessage="Reached monthly transaction limit for the Beneficiary" });
                    }
                }
                else
                {
                    return StatusCode(500, new ErrorDto { ErrorCode = 51, ErrorMessage = "Reached monthly transaction limit" });
                }
            }
        }
    }
}

