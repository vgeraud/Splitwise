using Splitwise.Models.Enums;
using System;

namespace Splitwise.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ExpenseType Type { get; set; }
        public DateTime Date { get; set; }
        public User Payer { get; set; }
        public double InitialAmount { get; set; }
        public double CurrentAmount { get; set; }
        public Currency Currency { get; set; }
        public bool IsTaxIncluded { get; set; }
    }
}