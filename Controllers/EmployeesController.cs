using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocsService.Data;
using DocsService.Models;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;

namespace DocsService.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class EmployeesController: ControllerBase
    {
        private AppDbContext _context;

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
            e.BirthDate,
            e.FirstName,
            e.LastName,
            e.MiddleName
        })
        .ToListAsync();

            return new JsonResult(employees);
        }

        [HttpPost("UpdateEmployees")]
        public async Task<IActionResult> UpdateEmployees([FromBody] List<Employees> employees)
        {
            //if (employees == null || !employees.Any())
            //{
            //    return BadRequest("Нет данных для обновления");
            //}
            foreach (var emp in employees)
            {
                var empDB = await _context.Employees.FindAsync(emp.ID);
                if (empDB != null)
                {
                    empDB.LastName = emp.LastName;
                    empDB.FirstName = emp.FirstName;
                    empDB.MiddleName = emp.MiddleName;
                    empDB.BirthDate = emp.BirthDate;
                    empDB.Position = emp.Position;
                    empDB.Email_User = emp.Email_User;
                }
                else if (emp.ID == -1)
                {
                    var newEmpDB = new Employees
                    {
                        LastName = emp.LastName,
                        FirstName = emp.FirstName,
                        MiddleName = emp.MiddleName,
                        BirthDate = emp.BirthDate,
                        Position = emp.Position,
                        Email_User = emp.Email_User,
                    };

                    _context.Employees.Add(newEmpDB);
                }
                
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("deleteRow")]
        public async Task<IActionResult> DeleteRow(int id)
        {
            var empDB = await _context.Employees.FindAsync(id);
            if (empDB == null) { return BadRequest($"Сотрудник с ID {id} не найден"); }

            _context.Employees.Remove(empDB);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Успешно удален", id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            
            return await DeleteRow(id); 
        }
    }
}
