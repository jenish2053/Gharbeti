using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gharbetti.Models
{
    public class TransactionDetail
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Transaction")]
        public int TransactionId { get; set; }
        [ForeignKey("Expense")]
        public int ExpenseId { get; set; }
        public decimal Amount { get; set; }
        public string? Remarks { get; set; }
        public Transaction Transaction { get; set; }
        public ExpenseType Expense { get; set; }
    }
}