using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gharbetti.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Floor")]
        public int FloorId { get; set; }
        public string RoomNo { get; set; }
        public decimal RentAmount { get; set; }
        public string? SquareFootage { get; set; }
        public string Remarks { get; set; }
        public Floor Floor { get; set; }
    }
}