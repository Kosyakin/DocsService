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
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
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
                                today,
                                employees
                            );
                            
                            manager.OTseptember = true;
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
                                today,
                                employees
                            );
                            manager.OTmarch = true;
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
                                today,
                                employees
                            );
                            manager.PBseptember = true;
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
    }
}
