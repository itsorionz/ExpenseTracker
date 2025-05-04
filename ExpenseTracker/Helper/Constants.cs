using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Helper
{
    public static class Constants
    {
        public static List<string> TransactionTypes { get; } = new() { "Income", "Expense" };
    }
}
