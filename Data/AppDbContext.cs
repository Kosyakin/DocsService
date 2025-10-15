﻿using DocsService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocsService.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employees> Employees { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employees>().HasData(
                new Employees
                {
                    ID = 1,
                    LastName = "Иванов",
                    FirstName = "Иван",
                    MiddleName = "Иванович",
                    BirthDate = new DateTime(1985, 5, 15),
                    Position = "Инженер",
                    Email_User = "kurdina.00@gmail.com"
                },
                new Employees
                {
                    ID = 2,
                    LastName = "Петров",
                    FirstName = "Петр",
                    MiddleName = "Петрович",
                    BirthDate = new DateTime(1990, 8, 22),
                    Position = "Механик",
                    Email_User = "login.00@gmail.com"
                });

            //modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        Id = 1,
            //        Email = "ivanov.com",
            //        Password = "12345",
            //        Name = "Иванов Иван Иванович",
            //        Post = "Начальник отдела 1",
            //        umDoc = "123456789"
            //    },
            //    new User
            //    {
            //        Id = 2,
            //        Email = "petrov.com",
            //        Password = "6789",
            //        Name = "Петров Петр Петрович",
            //        Post = "Начальник отдела 2",
            //        numDoc = "987654321"
            //    });

            //modelBuilder.Entity<User>()
            //    .HasIndex(u => u.Id)
            //    .IsUnique();
        }
    }
}
