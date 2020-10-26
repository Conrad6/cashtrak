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
        public CashTrakContext(DbContextOptions options):base(options)
        {
            var created = this.Database.EnsureCreated();
        }

        public virtual DbSet<MonthlyBudget> MonthlyBudgets { get; set; }
        public virtual DbSet<Expense> BudgetExpenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MonthlyBudget>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.BudgetDate).IsRequired();
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

            #endregion
        }
    }

    internal class DateTimeGenerator : ValueGenerator<DateTime>
    {
        public override DateTime Next(EntityEntry entry) => DateTime.Now;

        public override bool GeneratesTemporaryValues => false;
    }
}