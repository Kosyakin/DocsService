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

            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{count} пользователей успешно удалены." });
        }
    }
}
