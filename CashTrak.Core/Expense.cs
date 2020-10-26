using System;

namespace CashTrak.Core
{
    public class Expense
    {
        public Guid Id { get; set; }
        public Guid? BudgetId { get; set; }
        public MonthlyBudget Budget { get; set; }
        public double? ExpenseAmount { get; set; }
        public string Description { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}