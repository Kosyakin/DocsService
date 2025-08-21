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



// аутентификация с помощью куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    options.LoginPath = "/Account/Login");
builder.Services.AddAuthorization();



var app = builder.Build();



app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();



app.MapControllers();

app.Run();