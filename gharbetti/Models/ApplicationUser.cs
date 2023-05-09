using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gharbetti.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public DateTime Dob { get; set; }
        public string? Identification { get; set; }
        public string? PhotoId { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        public string? County { get; set; }
        [Required]
        public string Country { get; set; }
        [ForeignKey("Room")]
        public int? HouseRoomId { get; set; }
        public string? StayLength { get; set; }

        public string? ApproveRemarks { get; set; }
        public string? CustomerId { get; set; }
    }
}
