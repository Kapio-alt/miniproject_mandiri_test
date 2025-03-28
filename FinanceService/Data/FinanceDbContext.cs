using FinanceService.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Data
{
    public class FinanceDbContext:DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options) { }

        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Budget> Budgets { get; set; }
    }
}
