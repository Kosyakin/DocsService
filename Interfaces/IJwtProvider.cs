using DocsService.Models;

namespace DocsService.Interfaces
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
    }
}
