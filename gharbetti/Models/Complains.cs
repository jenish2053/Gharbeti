using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gharbetti.Models
{
    public class Complain
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string TenantId { get; set; }
        public string Reason { get; set; }
        public string? Response { get; set; }
        public DateTime ComplainDate { get; set; }
        public byte Status { get; set; }
        public ApplicationUser User { get; set; }
    }
}