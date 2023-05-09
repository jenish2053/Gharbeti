using System.ComponentModel.DataAnnotations;

namespace Gharbetti.ViewModels
{
    public class ComplainViewModel
    {
        public int Id { get; set; }
        public string TenantId { get; set; }
        public string Reason { get; set; }
        public string? Response { get; set; }
        public DateTime ComplainDate { get; set; }
        public byte Status { get; set; }
    }
}