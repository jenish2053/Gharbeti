using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gharbetti.Models
{
    public class PayementDetail
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Transaciton")]
        public int TransactionId { get; set; }


        [Required]
        public int PaymentModeId { get; set; }

        [Required]
        public string? Remarks { get; set; }

        [Required]
        public decimal PaidAmount { get; set; }
        public string TransactionNumber { get; set; }
        public Transaction Transaction { get; set; }

    }
}