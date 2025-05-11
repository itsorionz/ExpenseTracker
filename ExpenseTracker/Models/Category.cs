using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotMapped]
        public int Sl { get; set; }
        public string Type { get; set; } 
        public string CategoryName { get; set; }
        public bool IsSynced { get; set; }
    }
}
