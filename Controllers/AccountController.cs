using DocsService.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace DocsService.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly Dictionary<string, string> _employees = new Dictionary<string, string>
        {
            {"user1", "12345" },
            {"user2", "6789" }
        };
        
        // Страница авторизации
        [HttpGet("/")]
        public IActionResult Login()
        {
            return PhysicalFile(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/authorization/authorization.html"),
        "text/html"
    );
        }

        // Проверка логина/пароля
        [HttpPost("Account/Login")]
        public async Task<IActionResult> Login(string login, string password)
        {

            if (_employees.TryGetValue(login, out var correctPassword) && password == correctPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login),
                    
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return PhysicalFile(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/form/form.html"),
                    "text/html");
            }

            // если ошибка
            TempData["Error"] = "Неверный логин или пароль";
            return Redirect("/");
        }

        // Выход
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }

}
