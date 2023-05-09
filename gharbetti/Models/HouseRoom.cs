using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gharbetti.Models
{
    public class HouseRoom
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("House")]
        public int HouseId { get; set; }
        [ForeignKey("Room")]
        public int RoomId { get; set; }

        public House House { get; set; }
        public Room Room { get; set; }
    }
}