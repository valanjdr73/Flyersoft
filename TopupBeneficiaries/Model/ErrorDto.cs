using System;
namespace TopupBeneficiaries.Model
{
	public class ErrorDto
	{
		public ErrorDto()
		{
		}

		public int ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
	}
}

