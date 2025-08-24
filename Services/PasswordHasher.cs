using DocsService.Interfaces;

namespace DocsService.Services
{
    public class PasswordHasher: IPasswordHasher
    {
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);


        public bool Verify(string password, string hushedPassword) =>
                BCrypt.Net.BCrypt.EnhancedVerify(password, hushedPassword);
    }
}
