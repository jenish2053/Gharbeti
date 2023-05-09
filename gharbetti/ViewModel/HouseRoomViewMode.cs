using System.ComponentModel.DataAnnotations;

namespace Gharbetti.ViewModels
{
    public class HouseRoomViewModel
    {
        public int Id { get; set; }
        public int HouseId { get; set; }
        public string HouseName { get; set; } = string.Empty;

        public int RoomId { get; set; }
        public string RoomName { get; set;} = string.Empty;
    }
}