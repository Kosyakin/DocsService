using DocsService.Data;
using DocsService.Interfaces;
using DocsService.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace DocsService.Services
{
    public class TrainingReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public TrainingReminderService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Подождать первую минуту
            await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAndSendRemindersAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в сервисе напоминаний: {ex}");
                }

                // Подождать 24 часа до следующей проверки
                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }

        private async Task CheckAndSendRemindersAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var today = DateTime.Today;

            // Загружаем руководителей с датой напоминания
            var managers = await dbContext.Users
                .Where(u => dbContext.Employees.Any(e => e.Email_User == u.Email))
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.ReminderDateOTseptember
                })
                .Distinct()
                .ToListAsync();

            if (!managers.Any()) return;

            foreach (var manager in managers)
            {
                var reminderDate = manager.ReminderDateOTseptember;
                if (reminderDate.HasValue)
                {
                    var start = reminderDate.Value.Date;
                    var end = start.AddDays(10);
                    if (today >= start && today <= end)
                    {
                        var employees = await dbContext.Employees
                            .Where(e => e.Email_User == manager.Email)
                            .ToListAsync();

                        try
                        {
                            await emailService.SendReminderAsync(
                                manager.Email,
                                "проведение повторного инструктажа по ОТ",
                                today,
                                employees
                            );
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }
                }
                
            }
        }
    }
}
