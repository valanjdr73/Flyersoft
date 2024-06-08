using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopupBeneficiaries.Entities
{
	public class TopUpOption
	{
		public TopUpOption()
		{
            Name = string.Empty;
		}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal TopUpAmount { get; set; }
        public bool IsDeleted { get; set; }

        public List<TopUpTransaction>? TopUpTransactions { get; set; }


        public DateTime CreatedDateTime { get; set; }
        public DateTime LastUpdateDateTime { get; set; }

    }
}

