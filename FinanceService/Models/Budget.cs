namespace FinanceService.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public decimal Limit { get; set; }
        public int UserId { get; set; } // Foreign Key (from AuthService)
    }

    public class ReqBudget
    {
        public string Category { get; set; }
        public decimal Limit { get; set; }
        public int UserId { get; set; } // Foreign Key (from AuthService)
    }
}
