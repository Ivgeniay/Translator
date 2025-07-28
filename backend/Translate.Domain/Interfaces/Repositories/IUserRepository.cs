using Translate.Domain.Entities;

namespace Translate.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserProfile?> GetByIdAsync(string id);
        Task<UserProfile?> GetByUsernameAsync(string username);
        Task<UserProfile?> GetByEmailAsync(string email);
        Task<List<UserProfile>> GetActiveUsersAsync();
        Task<List<UserProfile>> GetUsersByStatusAsync(UserStatus status);
        Task AddAsync(UserProfile user);
        Task UpdateAsync(UserProfile user);
        Task DeleteAsync(string id);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);
    }
}