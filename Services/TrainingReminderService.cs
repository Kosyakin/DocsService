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
        private DateTime _lastResetCheck = DateTime.Today;


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
                    await CheckAndResetYearlyFlagsAsync();
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

            // Загружаем руководителей с датой напоминания
            var managers = await dbContext.Users
    .Where(u => dbContext.Employees
        .Select(e => e.Email_User)
        .Contains(u.Email))
    .ToListAsync();

            if (!managers.Any()) return;

            foreach (var manager in managers)
            {
                var reminderDate = manager.ReminderDateOTseptember;
                var reminderDate1 = manager.ReminderDateOTmarch;
                var reminderDate2 = manager.ReminderDatePBseptember;
                if (reminderDate.HasValue && reminderDate1.HasValue && reminderDate2.HasValue)
                {
                    var start = reminderDate.Value.Date;
                    var end = start.AddDays(10);

                    var start1 = reminderDate1.Value.Date;
                    var end1 = start1.AddDays(10);

                    var start2 = reminderDate2.Value.Date;
                    var end2 = start2.AddDays(10);


                    if (today >= start && today <= end && manager.OTseptember == false)
                    {
                        var employees = await dbContext.Employees
                            .Where(e => e.Email_User == manager.Email)
                            .ToListAsync();

                        try
                        {
                            await emailService.SendReminderAsync(
                                manager.Email,
                                "проведение повторного инструктажа по ОТ",
                                manager.ReminderDateOTseptember.GetValueOrDefault(),
                                employees
                            );

                            //manager.OTseptember = true;
                            dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }

                    if (today >= start1 && today <= end1 && manager.OTmarch == false)
                    {
                        var employees = await dbContext.Employees
                            .Where(e => e.Email_User == manager.Email)
                            .ToListAsync();

                        try
                        {
                            await emailService.SendReminderAsync(
                                manager.Email,
                                "проведение повторного инструктажа по ОТ",
                                manager.ReminderDateOTmarch.GetValueOrDefault(),
                                employees
                            );
                            //manager.OTmarch = true;
                            dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }

                    if (today >= start2 && today <= end2 && manager.PBseptember == false)
                    {
                        var employees = await dbContext.Employees
                            .Where(e => e.Email_User == manager.Email)
                            .ToListAsync();

                        try
                        {
                            await emailService.SendReminderAsync(
                                manager.Email,
                                "проведение повторного инструктажа по ППБ",
                                manager.ReminderDatePBseptember.GetValueOrDefault(),
                                employees
                            );
                            //manager.PBseptember = true;
                            dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }


                }

            }
        }

        private async Task CheckAndResetYearlyFlagsAsync()
        {
            var today = DateTime.Today;
            
            // Проверяем, наступил ли новый год (1 января) и мы еще не сбрасывали флаги в этом году
            if (today.Month == 1 && today.Day == 1 && today.Year != _lastResetCheck.Year)
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Сбрасываем все флаги для всех пользователей
                var allManagers = await dbContext.Users.ToListAsync();
                
                foreach (var manager in allManagers)
                {
                    manager.OTseptember = false;
                    manager.OTmarch = false;
                    manager.PBseptember = false;
                    
                    // обновляем даты напоминаний на новый год
                    if (manager.ReminderDateOTseptember.HasValue)
                        manager.ReminderDateOTseptember = new DateTime(today.Year, 9, 1);
                    
                    if (manager.ReminderDateOTmarch.HasValue)
                        manager.ReminderDateOTmarch = new DateTime(today.Year, 3, 1);
                    
                    if (manager.ReminderDatePBseptember.HasValue)
                        manager.ReminderDatePBseptember = new DateTime(today.Year, 9, 1);
                }

                await dbContext.SaveChangesAsync();
                _lastResetCheck = today;
                
                Console.WriteLine($"Флаги сброшены на новый {today.Year} год");
            }
        }
    }
}
