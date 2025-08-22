using Microsoft.AspNetCore.Identity;
namespace DocsService.Models
{
    public class ApplicationUser: IdentityUser
    {
        //public int Id { get; set; }

        public string Name { get; set; }

        //public string Login { get; set; }

        //public string Password { get; set; }

        public string Post {  get; set; }

        public string NumDoc { get; set; }

    }
}
