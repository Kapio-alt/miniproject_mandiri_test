namespace FinanceService.Models
{
    public class Income
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; } // Foreign Key (from AuthService)
    }
    public class ReqIncome
    {
        public string Source { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; } // Foreign Key (from AuthService)
    }
}
