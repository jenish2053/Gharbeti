using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class PaymentMode
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string? StripePayment { get; set; }
        
    }
}