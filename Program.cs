using DocsService.Data;
using DocsService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


//бд с пользователями
//var people = new List<Users>
//{
//    new Users("test1", "123456"),
//    new Users("test2", "567")

//}

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

// Настройка Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Настройка кук аутентификации
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

//Настройка аутентификации с куками
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken"; // Имя заголовка для CSRF-токена
    options.Cookie.Name = "CSRF-TOKEN"; // Имя куки
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Только HTTPS
    options.FormFieldName = "__RequestVerificationToken"; // Имя поля в форме
});

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

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

app.MapGet("/", () => Results.Redirect("/Account/Login"));
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();