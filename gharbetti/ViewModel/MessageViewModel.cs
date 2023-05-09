using System.ComponentModel.DataAnnotations;

namespace Gharbetti.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public bool IsAll { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; } 
        public DateTime PostedDate { get; set; }
        public List<MessageTenantViewModel> Tenant { get; set; }
    }
}