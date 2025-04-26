using FinanceService.Data;
using FinanceService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly FinanceDbContext _context;

        public BudgetController(FinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudgets(int userID)
        {
            var budgets = await _context.Budgets
                .Where(r => r.UserId == userID).ToListAsync();

            if (budgets == null)
            {
                return NotFound("No Budget found for this user.");
            }
            return budgets;
        }

        [HttpPost]
        public async Task<ActionResult<Budget>> AddBudget(ReqBudget req)
        {
            Budget budget = new Budget
            {
                Category = req.Category,
                Limit = req.Limit,
                UserId = req.UserId // Assuming UserId is passed in the request
            };
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBudgets), new { id = budget.Id }, budget);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(int id, ReqBudget req)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound("Budget not found.");
            }
            budget.Category = req.Category;
            budget.Limit = req.Limit;
            _context.Entry(budget).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound("Budget not found.");
            }
            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
