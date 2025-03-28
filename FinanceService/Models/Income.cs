namespace FinanceService.Models
{
    public class Income
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; } // Foreign Key (from AuthService)
    }
}
