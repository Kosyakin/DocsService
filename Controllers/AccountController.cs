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
            
            if (login == "admin" && password == "12345")
            {
                // создаём клеймы (инфу о пользователе)
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login)
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return PhysicalFile(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/form/form.html"),
        "text/html");
            }

            // если ошибка
            ViewBag.Error = "Неверный логин или пароль";
            return PhysicalFile(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/authorization/authorization.html"),
        "text/html");
        }

        // Выход
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }

    //public class AccountController : Controller
    //{
    //    private readonly SignInManager<IdentityUser> _signInManager;
    //    private readonly IAntiforgery _antiforgery;

    //    public AccountController(
    //        SignInManager<IdentityUser> signInManager,
    //        IAntiforgery antiforgery)
    //    {
    //        _signInManager = signInManager;
    //        _antiforgery = antiforgery;
    //    }

    //    [HttpGet("/test")]
    //    public IActionResult Test()
    //    {
    //        return Content("Test OK", "text/plain");
    //    }

    //    [HttpGet("Login")]
    //    public IActionResult Login()
    //    {
    //        Console.WriteLine("✅ AccountController.Login() вызван");
    //        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);

    //        // Устанавливаем куку с токеном
    //        Response.Cookies.Append(
    //            "XSRF-TOKEN",
    //            tokens.RequestToken,
    //            new CookieOptions
    //            {
    //                HttpOnly = false,
    //                Secure = false, // true только на HTTPS
    //                SameSite = SameSiteMode.Strict,
    //                Path = "/"
    //            }
    //        );

    //        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "authorization", "authorization.html");

    //        if (!System.IO.File.Exists(filePath))
    //        {
    //            Console.WriteLine("❌ Файл не найден: " + filePath);
    //            return NotFound();
    //        }

    //        var content = System.IO.File.ReadAllText(filePath);
    //        return Content(content, "text/html");
    //    }

    //    [HttpPost("Login")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Login([FromBody] LoginModel model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var result = await _signInManager.PasswordSignInAsync(
    //                model.Login,
    //                model.Password,
    //                isPersistent: false,
    //                lockoutOnFailure: false);

    //            if (result.Succeeded)
    //            {
    //                return Json(new { success = true });
    //            }
    //        }
    //        return Json(new { success = false, error = "Неверный логин или пароль" });
    //    }

    //    [HttpPost("Test")]
    //    public async Task<IActionResult> Test([FromBody] LoginModel model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var result = await _signInManager.PasswordSignInAsync(
    //                model.Login,
    //                model.Password,
    //                isPersistent: false,
    //                lockoutOnFailure: false);

    //            if (result.Succeeded)
    //            {
    //                return Json(new { success = true });
    //            }
    //        }
    //        return Json(new { success = false, error = "Неверный логин или пароль" });
    //    }
    //}

}
