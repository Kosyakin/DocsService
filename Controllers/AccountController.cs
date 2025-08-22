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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("/")]
        public IActionResult Login()
        {
            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/authorization/authorization.html"),
                "text/html"
            );
        }

        [HttpPost("Account/Login")]
        public async Task<IActionResult> Login(string login, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(login, password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return PhysicalFile(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/form/form.html"),
                    "text/html"
                );
            }

            TempData["Error"] = "Неверный логин или пароль";
            return Redirect("/");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }

}
