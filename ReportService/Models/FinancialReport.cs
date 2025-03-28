namespace ReportService.Models
{
    public class FinancialReport
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetSavings => TotalIncome - TotalExpenses;
        public DateTime ReportDate { get; set; } = DateTime.UtcNow;
    }
}
