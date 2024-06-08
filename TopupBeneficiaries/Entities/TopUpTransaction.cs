using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopupBeneficiaries.Entities
{
	public class TopUpTransaction
	{
		public TopUpTransaction()
		{
            User = new User();
            TopUpOption = new TopUpOption();
            TopUpBeneficiary = new TopUpBeneficiary();
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public long TopUpBeneficiaryId { get; set; }
        public TopUpBeneficiary TopUpBeneficiary { get; set; }

        public int TopUpOptionId { get; set; }
        public TopUpOption TopUpOption { get; set; }

        public DateTime TopupDateTime { get; set; }
        public decimal TopUpChargeAmount { get; set; }

    }
}

