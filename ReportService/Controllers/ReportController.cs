using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportService.Data;
using ReportService.Models;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly ReportDbContext _context;

        public ReportController(ReportDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary/{userId}")]
        public async Task<ActionResult<FinancialReport>> GetUserFinancialSummary(string userId)
        {
            var report = await _context.FinancialReports
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReportDate)
                .FirstOrDefaultAsync();

            if (report == null)
            {
                return NotFound("No financial report found for this user.");
            }

            return Ok(report);
        }

        [HttpPost("generate")]
        public async Task<ActionResult<FinancialReport>> GenerateReport(FinancialReport report)
        {
            _context.FinancialReports.Add(report);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserFinancialSummary), new { userId = report.UserId }, report);
        }
    }
}
