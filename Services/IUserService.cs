using DocsService.Data;
using DocsService.Models;
using Microsoft.EntityFrameworkCore;
namespace DocsService.Services
{
    public interface IUserService
    {
        Task<User?> Authenticate(string username, string password);
        Task<bool> CreateUser(string username, string password);
        Task<bool> UserExists(string username);
    }

    public class UserService: IUserService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;
    }
}
