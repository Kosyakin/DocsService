using DocsService.Data;
using DocsService.Models;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2013.Word;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


// условная бд с пользователями
var people = new List<Person>
{
    new Person("tom@gmail.com", "12345"),
    new Person("bob@gmail.com", "55555")
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Разрешаем запросы с любых доменов
              .AllowAnyMethod()  // Разрешаем все HTTP-методы (GET, POST и т.д.)
              .AllowAnyHeader()  // Разрешаем все заголовки
              .WithExposedHeaders("*");  // Разрешаем все доступные заголовки в ответе
    });
});

// получаем строку подключения из файла конфигурации
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));

//// Настройка Identity
//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequiredLength = 6;
//})
//.AddEntityFrameworkStores<AppDbContext>()
//.AddDefaultTokenProviders();

//// Настройка кук аутентификации
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.HttpOnly = true;
//    options.ExpireTimeSpan = TimeSpan.FromHours(1);
//    options.LoginPath = "/Account/Login";
//    options.AccessDeniedPath = "/Account/AccessDenied";
//    options.SlidingExpiration = true;
//});

////Настройка аутентификации с куками
//builder.Services.AddAntiforgery(options =>
//{
//    options.HeaderName = "RequestVerificationToken"; // Имя заголовка для CSRF-токена
//    options.Cookie.Name = "CSRF-TOKEN"; // Имя куки
//    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Только HTTPS
//    options.FormFieldName = "__RequestVerificationToken"; // Имя поля в форме
//});

// аутентификация с помощью куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    options.LoginPath = "/Account/Login");
builder.Services.AddAuthorization();



var app = builder.Build();


//app.UseCors("AllowAll");
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "Views/form/")),
//    RequestPath = "/form"
//});

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "Views/authorization/")),
//    RequestPath = "/authorization"
//});

app.MapControllers();

//app.MapGet("/", () => Results.Redirect("/Account/Login"));






app.Run();

record class Person(string Email, string Password);