using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Metrica.Authentication.Application.Dtos.Security;
using Metrica.Authentication.Application.Interfaces.Security;
using Metrica.Authentication.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Metrica.Authentication.Infrastructure.Security
{
    public sealed class JwtAccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly JwtOptions _options;

        public JwtAccessTokenGenerator(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public AccessTokenResult Generate(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var now = DateTime.UtcNow;
            var expiresAtUtc = now.AddMinutes(_options.ExpirationMinutes);

            var claims = new List<Claim>
            {
                new(
                    JwtRegisteredClaimNames.Sub,
                    user.Id.ToString(CultureInfo.InvariantCulture)),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("name", user.FullName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
            };

            if (user.CanUploadFiles)
            {
                claims.Add(new Claim("permission", "fileloads:upload"));
            }

            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SigningKey));

            var credentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: expiresAtUtc,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return new AccessTokenResult(accessToken, expiresAtUtc);
        }
    }
}