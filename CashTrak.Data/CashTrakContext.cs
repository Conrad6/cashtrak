using System;
using System.Linq;
using Bogus;
using CashTrak.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CashTrak.Data
{
    public class CashTrakContext : DbContext
    {
        public CashTrakContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<MonthlyBudget> MonthlyBudgets { get; set; }
        public virtual DbSet<Expense> BudgetExpenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MonthlyBudget>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Month).IsRequired();
                b.Property(x => x.DateAdded)
                    .HasValueGenerator<DateTimeGenerator>()
                    .ValueGeneratedOnAdd();
                b.Property(x => x.LastUpdated)
                    .HasValueGenerator<DateTimeGenerator>()
                    .ValueGeneratedOnAddOrUpdate();
                b.Property(x => x.Budget).IsRequired();
                b.HasMany(x => x.Expenses)
                    .WithOne(x => x.Budget)
                    .HasForeignKey(x => x.BudgetId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(false);
            });

            modelBuilder.Entity<Expense>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Budget)
                    .WithMany(x => x.Expenses)
                    .HasForeignKey(x => x.BudgetId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);
                b.Property(x => x.ExpenseAmount).IsRequired();
                b.Property(x => x.Description).HasMaxLength(600);
                b.Property(x => x.ExpenseDate).IsRequired();
                b.Property(x => x.DateCreated)
                    .HasValueGenerator<DateTimeGenerator>()
                    .ValueGeneratedOnAdd();
                b.Property(x => x.LastUpdated)
                    .HasValueGenerator<DateTimeGenerator>()
                    .ValueGeneratedOnAddOrUpdate();
            });

            #region Seed

            var expensesFaker = new Faker<Expense>()
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Description, f => f.Lorem.Lines(f.Random.Number(2)));

            var budgetFaker = new Faker<MonthlyBudget>()
                .RuleFor(m => m.Month, f => f.PickRandom<Month>())
                .RuleFor(m => m.Budget, f => f.Random.Double() * f.Random.Number(1000))
                .RuleFor(m => m.DateAdded,
                    (f, m) => new DateTime(f.Random.Number(10), (int) m.Month.Value,
                        f.Random.Number(m.Month?.GetLength() ?? 31)));

            var budgets = budgetFaker.Generate(10);

            expensesFaker = expensesFaker
                .RuleFor(e => e.BudgetId, f => f.PickRandom(budgets.Select(x => x.Id)))
                .RuleFor(e => e.ExpenseAmount,
                    (f, m) => f.Random.Double(0D, budgets.Single(x => x.Id == m.BudgetId.Value).Budget ?? 0D))
                .RuleFor(e => e.ExpenseDate, (f, e) => f.Date.Between(
                    budgets.Single(x => x.Id == e.BudgetId).DateAdded ?? DateTime.Now,
                    budgets.Single(x => x.Id == e.BudgetId).DateAdded
                        ?.AddDays((double) budgets.Single(x => x.Id == e.BudgetId).Month?.GetLength()) ??
                    DateTime.Now));
            
            var expenses = expensesFaker.Generate(new Random().Next(1, 20));

            modelBuilder.Entity<MonthlyBudget>().HasData(budgets);
            modelBuilder.Entity<Expense>().HasData(expenses);

            #endregion
        }
    }

    internal class DateTimeGenerator : ValueGenerator<DateTime>
    {
        public override DateTime Next(EntityEntry entry) => DateTime.Now;

        public override bool GeneratesTemporaryValues => false;
    }
}