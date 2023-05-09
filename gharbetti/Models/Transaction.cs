using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gharbetti.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string TenantId { get; set; }
        public decimal Total { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Remarks { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RentAmount { get; set; }
        public decimal RentPaid { get; set; }
        [ForeignKey("PaymentMode")]
        public int PaymentModeId { get; set; }  
        public string? StripePaymentId { get; set; }
        public PaymentMode PaymentMode { get; set; }
        public ApplicationUser User { get; set; }   

    }
}