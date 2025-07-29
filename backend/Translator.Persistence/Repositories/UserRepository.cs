using Translator.Domain.Entities;
using Translator.Domain.Interfaces.Repositories;

namespace Translator.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task AddAsync(UserProfile user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserProfile>> GetActiveUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserProfile?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfile?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfile?> GetByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserProfile>> GetUsersByStatusAsync(UserStatus status)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserProfile user)
        {
            throw new NotImplementedException();
        }
    }
}