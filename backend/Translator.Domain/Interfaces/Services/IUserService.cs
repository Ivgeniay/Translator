using Translator.Domain.Entities;

namespace Translator.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserProfile> CreateUserAsync(string? username = null, string? email = null);
        Task<UserProfile?> GetUserAsync(string userId);
        Task<UserProfile?> GetUserByUsernameAsync(string username);
        Task<UserProfile?> GetUserByEmailAsync(string email);
        Task UpdateUserSettingsAsync(string userId, UserSettings settings);
        Task DeactivateUserAsync(string userId);
        Task<bool> ValidateUserAsync(string userId);
        Task UpdateLastLoginAsync(string userId);
    }
}