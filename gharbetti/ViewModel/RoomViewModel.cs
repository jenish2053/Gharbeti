using System.ComponentModel.DataAnnotations;

namespace Gharbetti.ViewModels
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public int FloorId { get; set; }
        public string RoomNo { get; set; }
        public decimal RentAmount { get; set; }
        public string? SquareFootage { get; set; }
        public string Remarks { get; set; }
        public List<RoomDetailViewModel> RoomDetails { get; set; } 
    }
}