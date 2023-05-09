using System.ComponentModel.DataAnnotations;

namespace Gharbetti.Models
{
    public class ExpenseType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}