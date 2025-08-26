using System.ComponentModel.DataAnnotations;
namespace DocsService.Contracts
{
    public record LoginUsersRequest(
        [Required] string Email,
        [Required] string Password);
}
