using DocsService.Models;

namespace DocsService.Interfaces
{
    public interface IUsersRepository
    {
        public Task Add(User user);
        public Task<User> GetByEmail(string email);
        Task<UserEntity?> GetById(Guid id);
    }
}
