using System.ComponentModel.DataAnnotations;

namespace Gharbetti.ViewModels
{
    public class CleanScheduleViewModel
    {
        public int Id { get; set; }
        public string TenantId { get; set; }
        public string Remarks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
    }
}