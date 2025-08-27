using System.ComponentModel.DataAnnotations;

namespace DocsService.Contracts
{
    public record saveReminderRequest(
        [Required] string Email,
        [Required] string reminderDate
        );
}
