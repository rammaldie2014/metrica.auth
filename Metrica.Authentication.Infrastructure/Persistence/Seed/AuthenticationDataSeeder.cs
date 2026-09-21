using Metrica.Authentication.Application.Interfaces.Security;
using Metrica.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metrica.Authentication.Infrastructure.Persistence.Seed
{
    public sealed class AuthenticationDataSeeder
    {
        private readonly AuthenticationDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationDataSeeder(
            AuthenticationDbContext dbContext,
            IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedUserAsync(
            string email,
            string fullName,
            string password,
            bool canUploadFiles,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            var normalizedEmail = email.Trim();

            var userExists = await _dbContext.Users.AnyAsync(
                user => user.Email == normalizedEmail,
                cancellationToken);

            if (userExists)
            {
                return;
            }

            var passwordHash = _passwordHasher.Hash(password);

            var user = new User(
                normalizedEmail,
                fullName,
                passwordHash,
                canUploadFiles);

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}