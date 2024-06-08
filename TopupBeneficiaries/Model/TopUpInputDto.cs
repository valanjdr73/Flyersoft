using System;
using System.ComponentModel.DataAnnotations;

namespace TopupBeneficiaries.Model
{
	public class TopUpInputDto
	{
		public TopUpInputDto()
		{
		}

		[Required]
		public long TopUpBeneficiaryId { get; set; }
		[Required]
		public int TopUpOptionId { get; set; }
	}
}

