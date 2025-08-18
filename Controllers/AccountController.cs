using DocsService.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DocsService.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAntiforgery _antiforgery;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            IAntiforgery antiforgery)
        {
            _signInManager = signInManager;
            _antiforgery = antiforgery;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Генерируем CSRF-токен для формы
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            ViewBag.CsrfToken = tokens.RequestToken;
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken] // Проверяем CSRF-токен
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Username,
                    model.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false, error = "Неверный логин или пароль" });
        }
    }
}
