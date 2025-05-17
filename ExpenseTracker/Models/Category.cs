using Newtonsoft.Json;
using SQLite;

namespace ExpenseTracker.Models
{
    public class Category : Base
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Ignore]
        [JsonIgnore]
        public int Sl { get; set; }
        public string Type { get; set; } 
        public string CategoryName { get; set; }
        public bool IsSynced { get; set; }
    }
}
