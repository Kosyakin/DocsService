using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocsService.Data;
using DocsService.Models;
using System.Runtime.InteropServices;

namespace DocsService.Controllers
{
    [Route("[controller]")]
    public class EmployeesController: ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Employees")]
        public async Task<ActionResult<IEnumerable<Employees>>> GetEmployees()
        {
            var employees = await _context.Employees
        .Select(e => new {
            e.ID,
            e.FullName,
            e.Position,
            e.BirthDate
        })
        .ToListAsync();

            return new JsonResult(employees);
        }
    }
}
