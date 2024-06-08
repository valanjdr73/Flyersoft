using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace TopupBeneficiaries.Entities
{
   
    public class TopUpBeneficiary
	{
		public TopUpBeneficiary()
        {
            NickName = string.Empty;
            PhoneNumber = string.Empty;
            User = new User();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }
        
        public User User { get; set; }

        [Required]
        [MaxLength(20)]
        public string NickName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public List<TopUpTransaction>? TopUpTransactions { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }

    }
}

