using FinanceService.Data;
using FinanceService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Controllers
{
    [Route("api/finance/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly FinanceDbContext _context;

        public ExpenseController(FinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses(int userID)
        {

            var expenses = await _context.Expenses
                .Where(r => r.UserId == userID).ToListAsync();

            if (expenses == null)
            {
                return NotFound("No expenses found for this user.");
            }
            return expenses;
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> AddExpense(ReqExpense req)
        {

            var budgets = await _context.Budgets
                .Where(r => r.UserId == req.UserId).ToListAsync();

            if(budgets == null)
            {
                return BadRequest("No Budget found for this user.");
            }

            foreach (var budget in budgets)
            {
                if (budget.Category == req.Category)
                {
                    if (budget.Limit < req.Amount)
                    {
                        return BadRequest("Expense exceeds budget limit.");
                    }
                }
            }

            var income = await _context.Incomes
                .Where(r => r.UserId == req.UserId).ToListAsync();

            if (income == null)
            {
                return BadRequest("No Income found for this user.");
            }

            foreach (var inc in income)
            {
                if (inc.Amount < req.Amount)
                {
                    return BadRequest("Expense exceeds income.");
                }
            }

            Expense expense = new Expense
            {
                Category = req.Category,
                Amount = req.Amount,
                Date = req.Date,
                UserId = req.UserId
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetExpenses), new { id = expense.Id }, expense);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteExpense(int expenseID)
        {
            _context.Remove(new Expense { Id = expenseID });
            await _context.SaveChangesAsync();
            return true;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, ReqExpense req)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound("Expense not found.");
            }
            expense.Category = req.Category;
            expense.Amount = req.Amount;
            expense.Date = req.Date;
            _context.Entry(expense).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
