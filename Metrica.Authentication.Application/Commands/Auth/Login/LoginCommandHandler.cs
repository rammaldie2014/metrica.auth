using Metrica.Authentication.Application.Dtos.Security;
using Metrica.Authentication.Application.Interfaces.Repositories;
using Metrica.Authentication.Application.Interfaces.Security;

namespace Metrica.Authentication.Application.Commands.Auth.Login
{
    public sealed class LoginCommandHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IAccessTokenGenerator accessTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<AccessTokenResult?> HandleAsync(
            LoginCommand command,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(command.Email) ||
                string.IsNullOrWhiteSpace(command.Password))
            {
                return null;
            }

            var user = await _userRepository.GetByEmailAsync(
                command.Email.Trim(),
                cancellationToken);

            if (user is null || !user.IsActive)
            {
                return null;
            }

            if (!_passwordHasher.Verify(
                command.Password,
                user.PasswordHash))
            {
                return null;
            }

            return _accessTokenGenerator.Generate(user);
        }
    }
}