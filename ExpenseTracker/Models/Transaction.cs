using SQLite;

namespace ExpenseTracker.Models
{
    public class Transaction : Base
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Type { get; set; } 
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
    }
}
