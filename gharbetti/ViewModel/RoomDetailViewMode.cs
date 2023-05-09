using System.ComponentModel.DataAnnotations;

namespace Gharbetti.ViewModels
{
    public class RoomDetailViewModel
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; }
        public string? SquareFootage { get; set; }
    }
}