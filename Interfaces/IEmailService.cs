namespace DocsService.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendReminderAsync(string email, string reminderType, DateTime reminderDate);
    }
}
