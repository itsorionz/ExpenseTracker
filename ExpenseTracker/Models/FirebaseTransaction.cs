﻿using SQLite;

namespace ExpenseTracker.Models
{
    public class FirebaseTransaction
    {
        public int Id { get; set; }
        public string Type { get; set; } 
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
