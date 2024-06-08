using System;
using System.ComponentModel.DataAnnotations;

namespace TopupBeneficiaries.Model
{
	public class AddBeneficiaryDto
	{
		public AddBeneficiaryDto()
		{

		}

		[Required]
		public long UserId { get; set; }
		[Required]
		[MaxLength(20)]
		public string NickName { get; set; }
		[Required]
		[Phone]
		public string PhoneNumber { get; set; }
	}
}

