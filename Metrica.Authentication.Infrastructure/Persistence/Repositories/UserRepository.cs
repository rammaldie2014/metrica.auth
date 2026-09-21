using Metrica.Authentication.Application.Interfaces.Repositories;
using Metrica.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metrica.Authentication.Infrastructure.Persistence.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly AuthenticationDbContext _dbContext;

        public UserRepository(AuthenticationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);

            var normalizedEmail = email.Trim();

            return await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    user => user.Email == normalizedEmail,
                    cancellationToken);
        }
    }
}