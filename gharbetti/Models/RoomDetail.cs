using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gharbetti.Models
{
    public class RoomDetail
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        [ForeignKey("RoomType")]
        public int RoomTypeId { get; set; }
        public string? SquareFootage { get; set; }
        public Room Room { get; set; }
        public RoomType RoomType { get; set; }  
    }
}