using System.ComponentModel.DataAnnotations;

namespace Gharbetti.ViewModels
{
    public class FilterViewModel
    {
        public string? StartDateString { get; set; } 
        public string? EndDateString { get; set; } 
        public int Month { get; set; }
        public string Year { get; set; }
        public int FilterType { get; set; }
        public string? PaidStatus { get; set; }
    }
}