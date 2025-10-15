using DocsService.Data;
using DocsService.Endpoints;
using DocsService.Extentions; // Добавьте эту директиву
using DocsService.Interfaces;
using DocsService.Models;
using DocsService.Repositories;
using DocsService.Services;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2013.Word;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
var serviceProvider = builder.Services.BuildServiceProvider();
var jwtOptions = serviceProvider.GetService<IOptions<JwtOptions>>();
builder.Services.AddApiAuthentication(jwtOptions);

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
// Переключаемся на SQLite в качестве провайдера БД
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection));

// Настраиваем постоянное хранилище ключей защиты данных внутри контейнера
var keysPath = Path.Combine(AppContext.BaseDirectory, "keys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("DocsService");

//var services = builder.Services;
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<UserService>();

builder.Services.AddHostedService<TrainingReminderService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DocsService API V1");
        options.RoutePrefix = "swagger"; // Доступ по /swagger
    });
}

app.UseRouting();
app.UseCors("AllowAll");


app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapUsersEndpoints();

app.MapGet("get", () =>
{
    return Results.Ok("ok");
}).RequireAuthorization();


app.MapGet("/account", async (HttpContext context, UserService userService) =>
{
    if (!context.User.Identity.IsAuthenticated)
    {
        return Results.Unauthorized();
    }

    var userIdClaim = context.User.FindFirst("userId");
    if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
    {
        return Results.Unauthorized();
    }

    var user = await userService.GetUserById(userId);

    var htmlContent = await System.IO.File.ReadAllTextAsync("wwwroot/form/form.html");

    // Заменяем плейсхолдеры реальными данными
    htmlContent = htmlContent
    .Replace("{{Email}}", user.Email)
    .Replace("{{UserName}}", $"{user.LastName} {user.FirstName} {user.MiddleName}")
        .Replace("{{Position}}", user.Position)
        .Replace("{{DocumentNumber}}", user.DocumentNumber)
        .Replace("{{Email}}", user.Email)
        .Replace("{{TrainingReminderDate}}", user.ReminderDateOTseptember?.ToString("yyyy-MM-dd") ?? "")
        .Replace("{{TrainingReminderDateOT2}}", user.ReminderDateOTmarch?.ToString("yyyy-MM-dd") ?? "")
        .Replace("{{TrainingReminderDatePB}}", user.ReminderDatePBseptember?.ToString("yyyy-MM-dd") ?? "");

    return Results.Content(htmlContent, "text/html");
}).RequireAuthorization();

// Обеспечиваем создание/инициализацию БД и схемы при старте приложения
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();