using FinanceService.Data;
using FinanceService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly FinanceDbContext _context;

        public IncomeController(FinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Income>>> GetIncomes(int userID)
        {
            var incomes = await _context.Incomes
                .Where(r => r.UserId == userID).ToListAsync();

            if (incomes == null)
            {
                return NotFound("No incomes found for this user."); 
            }
            return incomes;
        }

        [HttpPost]
        public async Task<ActionResult<Income>> AddIncome(ReqIncome req)
        {
            Income income = new Income
            {
                Source = req.Source,
                Amount = req.Amount,
                Date = req.Date,
                UserId = req.UserId
            };
            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetIncomes), new { id = income.Id }, income);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncome(int id, ReqIncome req)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return NotFound("Income not found.");
            }
            income.Source = req.Source;
            income.Amount = req.Amount;
            income.Date = req.Date;
            _context.Entry(income).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteIncome(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return NotFound("Income not found.");
            }
            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
