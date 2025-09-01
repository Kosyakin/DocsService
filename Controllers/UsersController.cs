using DocsService.Contracts;
using DocsService.Data;
using DocsService.Interfaces;
using DocsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DocsService.Controllers
{
    [Route("Users")]
    public class UsersController: ControllerBase
    {
        private AppDbContext _context;
        private IUsersRepository _usersRepository;

        public UsersController(AppDbContext context, IUsersRepository usersRepository)
        {
            _context = context;
            _usersRepository = usersRepository;
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {

            return await DeleteRowsByEmail(email);
        }

        [HttpGet("Users")]
        public async Task<ActionResult<IEnumerable<UserEntity>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        private async Task<IActionResult> DeleteRowsByEmail(string email)
        {
            var users = _context.Users.Where(u => u.Email == email);
            var count = await users.CountAsync();
            

            if (count == 0)
            {
                return NotFound(new { message = "Пользователи с такой почтой не найдены." });
            }

            var employees = _context.Employees.Where(e => e.Email_User == email);
            _context.Users.RemoveRange(users);
            _context.Employees.RemoveRange(employees);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{count} пользователей успешно удалены." });
        }
        [HttpPost("saveReminder")]
        public async Task<IActionResult> SaveReminder([FromBody] saveReminderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Некорректные данные" });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound(new { message = "Пользователь не найден" });
            }

            user.ReminderDateOTseptember = DateTime.Parse(request.reminderDate);
            user.ReminderDateOTmarch = DateTime.Parse(request.reminderDate1);
            user.ReminderDatePBseptember = DateTime.Parse(request.reminderDate2);

            user.OTseptember = false;
        user.OTmarch = false;
        user.PBseptember = false;

        _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Дата сохранена" });
        }
        
    }
}
