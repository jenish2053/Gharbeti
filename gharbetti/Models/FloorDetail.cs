using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gharbetti.Models
{
    public class FloorDetail
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("House")]
        public int HouseId { get; set; }
        [ForeignKey("Floor")]
        public int FloorId { get; set; }
        public string Remarks { get; set; }
        public House House { get; set; }
        public Floor Floor { get; set; }
    }
}