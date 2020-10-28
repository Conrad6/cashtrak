using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashTrak.Core
{
    public class MonthlyBudget
    {
        public Guid Id { get; set; }
        public DateTime? BudgetDate { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? LastUpdated { get; set; }
        public double? Budget { get; set; }
        public IEnumerable<Expense> Expenses { get; set; }
    }

    public enum Month
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public static class MonthExtensions
    {
        public static int GetLength(this Month month) => month switch
        {
            Month.February => DateTime.IsLeapYear(DateTime.Now.Year) ? 29 : 28,
            Month.April => 30,
            Month.September => 30,
            Month.June => 30,
            Month.November => 30,
            _ => 31
        };
    }
}