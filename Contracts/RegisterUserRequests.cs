using Azure.Identity;
using System.ComponentModel.DataAnnotations;

namespace DocsService.Contracts
{
    public record RegisterUserRequests(
        [Required] string UserName,
        [Required] string Password,
        [Required] string Email,
        [Required] string FirstName,
        [Required] string LastName,
        string MiddleName,
        [Required] string Position,
        [Required] string DocumentNumber
        );
}
