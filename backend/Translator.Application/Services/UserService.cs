using Translator.Domain.Interfaces.Repositories;
using Translator.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Translator.Domain.Entities;

namespace Translator.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserProfile> CreateUserAsync(string? username = null, string? email = null)
        {
            if (!string.IsNullOrEmpty(username) && await _userRepository.ExistsByUsernameAsync(username))
            {
                throw new InvalidOperationException($"Пользователь с именем '{username}' уже существует");
            }

            if (!string.IsNullOrEmpty(email) && await _userRepository.ExistsByEmailAsync(email))
            {
                throw new InvalidOperationException($"Пользователь с email '{email}' уже существует");
            }

            var user = new UserProfile(username, email);
            await _userRepository.AddAsync(user);

            _logger.LogInformation("Создан новый пользователь: {UserId}", user.Id);
            return user;
        }

        public async Task<UserProfile?> GetUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<UserProfile?> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<UserProfile?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task UpdateUserSettingsAsync(string userId, UserSettings settings)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"Пользователь с ID '{userId}' не найден");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException("Невозможно обновить настройки неактивного пользователя");
            }

            user.UpdateSettings(settings);
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Обновлены настройки пользователя: {UserId}", userId);
        }

        public async Task DeactivateUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"Пользователь с ID '{userId}' не найден");
            }

            user.Deactivate();
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Пользователь деактивирован: {UserId}", userId);
        }

        public async Task<bool> ValidateUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            var user = await _userRepository.GetByIdAsync(userId);
            return user != null && user.IsActive;
        }

        public async Task UpdateLastLoginAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Попытка обновить время входа для несуществующего пользователя: {UserId}", userId);
                return;
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Попытка обновить время входа для неактивного пользователя: {UserId}", userId);
                return;
            }

            user.UpdateLastLogin();
            await _userRepository.UpdateAsync(user);

            _logger.LogDebug("Обновлено время последнего входа для пользователя: {UserId}", userId);
        }
    }
}