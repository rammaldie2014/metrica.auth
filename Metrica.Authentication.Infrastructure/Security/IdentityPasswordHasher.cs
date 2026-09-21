using Metrica.Authentication.Application.Interfaces.Security;
using Microsoft.AspNetCore.Identity;

namespace Metrica.Authentication.Infrastructure.Security
{
    public sealed class IdentityPasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _hasher = new();
        private readonly object _context = new();

        public string Hash(string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            return _hasher.HashPassword(_context, password);
        }

        public bool Verify(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(passwordHash))
            {
                return false;
            }

            var result = _hasher.VerifyHashedPassword(
                _context,
                passwordHash,
                password);

            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}