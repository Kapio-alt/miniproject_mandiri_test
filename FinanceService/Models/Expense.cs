namespace FinanceService.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; } // Foreign Key (from AuthService)
    }

    public class ReqExpense
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; } // Foreign Key (from AuthService)
    }
}
