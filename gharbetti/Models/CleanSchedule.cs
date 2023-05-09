using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gharbetti.Models
{
    public class CleanSchedule
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public string? Remarks { get; set; }
        public ApplicationUser User { get; set; }
    }
}