﻿
namespace ExpenseTracker.Helper
{
    public static class Constants
    {
        public static List<string> TransactionTypes { get; } = new() { "Income", "Expense" };
        public static List<string> TransactionCategory { get; } = new() { "Salary", "Market Cost", "Medicine", "Bus Rent", "Loan EMI", "Breakfast", "Snack", "Cigarette", "Recharge", "Case Cost" , "Others"};
    }
}
