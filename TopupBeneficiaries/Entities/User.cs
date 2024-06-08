using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TopupBeneficiaries.Entities
{
	public class User
	{
		public User()
		{
            UserName = string.Empty;
     	}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
        public decimal BalanceAmount { get; set; }

        public List<TopUpBeneficiary>? TopUpBeneficiaries;
        public bool IsVerifiedUser { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public List<TopUpTransaction>? TopUpTransactions { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }
    }
}

