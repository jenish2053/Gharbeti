using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}