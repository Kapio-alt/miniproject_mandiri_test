using FinanceService.Data;
using FinanceService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly FinanceDbContext _context;

        public IncomeController(FinanceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Income>>> GetIncomes()
        {
            return await _context.Incomes.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Income>> AddIncome(Income income)
        {
            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetIncomes), new { id = income.Id }, income);
        }
    }
}
