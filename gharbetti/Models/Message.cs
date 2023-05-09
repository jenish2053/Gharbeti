using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gharbetti.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("House")]
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsAll { get; set; }
    }
}