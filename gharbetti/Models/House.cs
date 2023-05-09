using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class House
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? PostalCode { get; set; }
        public string Street { get; set; }
        public string? SquareFootage { get; set; }
        public string? Remarks { get; set; }
        public decimal? RentAmount { get; set; }
        
    }
}