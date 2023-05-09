using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace Gharbetti.Models
{
    public class TenantMessage
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Message")]
        public int MessageId { get; set; }
        [ForeignKey("User")]
        public string TenantId { get; set; }
        public byte Status { get; set; }
        public Message Message { get; set; }
        public ApplicationUser User { get; set; }   
                
    }
}