using Microsoft.EntityFrameworkCore;
using DocsService.Models;

namespace DocsService.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employees> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>().HasData(
                new Employees
                {
                    ID = 1,
                    LastName = "Иванов",
                    FirstName = "Иван",
                    MiddleName = "Иванович",
                    BirthDate = new DateTime(1985, 5, 15),
                    Position = "Инженер"
                },
                new Employees
                {
                    ID = 2,
                    LastName = "Петров",
                    FirstName = "Петр",
                    MiddleName = "Петрович",
                    BirthDate = new DateTime(1990, 8, 22),
                    Position = "Механик"
                });
        }
    }
}
