using DocsService.Data;
using DocsService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Разрешить запросы с любых доменов
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.UseCors("AllowAll");
app.UseRouting();
app.UseCors("AllowAll");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Views/form/")),
    RequestPath = ""
});

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    db.Database.EnsureCreated(); // Создает БД и таблицы, если их нет

//    if (!db.Employees.Any())
//    {
//        db.Employees.AddRange(
//            new Employees
//            {
//                ID = 1,
//                LastName = "Иванов",
//                FirstName = "Иван",
//                MiddleName = "Иванович",
//                BirthDate = new DateTime(1985, 5, 15),
//                Position = "Инженер"
//            },
//            new Employees
//            {
//                ID = 2,
//                LastName = "Петров",
//                FirstName = "Петр",
//                MiddleName = "Петрович",
//                BirthDate = new DateTime(1990, 8, 22),
//                Position = "Механик"
//            }
//        );
//        db.SaveChanges();
//    }
//}

app.Run();