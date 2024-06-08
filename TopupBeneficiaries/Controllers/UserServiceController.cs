using System;
using Microsoft.AspNetCore.Mvc;
using TopupBeneficiaries.Repositories;
using TopupBeneficiaries.Model;
using TopupBeneficiaries.Entities;

namespace TopupBeneficiaries.Controllers
{
	[ApiController]
	[Route("api/UserService")]
	public class UserServiceController : ControllerBase
	{
		private readonly IUserRepository _userRepository;
		private readonly ILogger<UserServiceController> _logger;
		public UserServiceController(ILogger<UserServiceController> logger, IUserRepository userRepository)
		{
			_userRepository = userRepository;
			_logger = logger;
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> GetActiveBeneficiaries(long Id)
		{
			try
			{
				return Ok(await _userRepository.GetActiveBeneficiariesAsync(Id));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error Getting Active Beneficiaries");
				return StatusCode(500, new ErrorDto { ErrorCode=999, ErrorMessage= "Error Getting Active Beneficiaries" });
			}
		}

		[HttpPost]
		[Route("AddBeneficiary")]
		public async Task<IActionResult>AddBeneficiary(AddBeneficiaryDto beneficiaryDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(modelState: ModelState);
			}
			else
			{

				TopUpBeneficiary beneficiary = new TopUpBeneficiary()
				{
					NickName = beneficiaryDto.NickName,
					PhoneNumber = beneficiaryDto.PhoneNumber,
					UserId = beneficiaryDto.UserId,
					IsActive = true,
					CreatedDateTime = DateTime.UtcNow,
					LastUpdatedDateTime = DateTime.UtcNow
				};
				try
				{
					User? currentUser = await _userRepository.GetUserAndBeneficiariesAsync(beneficiaryDto.UserId);
					if (currentUser == null)
					{
						return NotFound(new ErrorDto { ErrorCode= 12, ErrorMessage="User not found"});
					}
					if (currentUser?.TopUpBeneficiaries?.Where(t => t.IsActive == true && t.IsDeleted == false).Count() >= 5)
					{
						return StatusCode(500, new ErrorDto { ErrorCode = 5, ErrorMessage = "More than 5 active beneficiaries are alreay added!" });
					}
					if (currentUser?.TopUpBeneficiaries?.Exists(t => t.PhoneNumber == beneficiary.PhoneNumber) == false)
					{
						if (_userRepository.AddBeneficiary(beneficiary, currentUser))
							return NoContent();
						else
							return StatusCode(500, new ErrorDto { ErrorCode = 10, ErrorMessage = "Error, Beneficiary is not added" });
					}
					else
					{
						return BadRequest(new ErrorDto { ErrorCode = 2, ErrorMessage = "Phone Number already exists for a Beneficiary" });
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error adding Beneficiary");
					return StatusCode(500, new ErrorDto { ErrorCode = 999, ErrorMessage = "System Error Adding Beneficiary" });
				}
			}
		}
	}
}

