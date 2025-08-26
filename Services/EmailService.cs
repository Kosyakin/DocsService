using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using DocsService.Interfaces;
using DocsService.Models;
using Microsoft.Extensions.Options;


namespace DocsService.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public async Task SendReminderAsync(string email, string reminderType, DateTime reminderDate, List<Employees> employees)
        {


            var subject = $"Напоминание: {reminderType}";

            var employeesListText = employees.Any()
                ? string.Join("\r\n", employees.Select(e => $"{e.LastName} {e.FirstName} {e.MiddleName} - {e.Position}"))
                : "Список сотрудников пуст.";

            var message = $@"Необходимо в указанные сроки ({reminderDate.ToString("dd.MM.yyyy")}-{reminderDate.AddDays(10).ToString("dd.MM.yyyy")})
провести повторный инструктаж для сотрудников:

{employeesListText}";

            await SendEmailAsync(email, subject, message);
        }
    }
}
