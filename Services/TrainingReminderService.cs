using DocsService.Data;
using DocsService.Interfaces;
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
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

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
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task CheckAndSendRemindersAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var today = DateTime.Today;

            if ((today.Month == 8 && today.Day == 25) || (today.Month == 8 && today.Day == 26))
            {
                var managers = await dbContext.Users
                .Where(u => dbContext.Employees.Any(e => e.Email_User == u.Email))
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.FirstName,
                    u.LastName
                })
                .Distinct()
                .ToListAsync();

                

                if (!managers.Any())
                {
                    return;
                }

                foreach (var manager in managers)
                {
                    var employees = await dbContext.Employees
                    .Where(e => e.Email_User == manager.Email)
                    .ToListAsync();
                    
                    try
                    {
                        await emailService.SendReminderAsync(manager.Email, "DocsService", today, employees);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
    }
}
