using Metrica.Authentication.Domain.Entities;

namespace Metrica.Authentication.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);
    }
}