using System.ComponentModel.DataAnnotations;

namespace DocsService.Contracts
{
    public record NotificationSettings (
        [Required] string Email,
        [Required] bool RemindersEnabled
        );
}
