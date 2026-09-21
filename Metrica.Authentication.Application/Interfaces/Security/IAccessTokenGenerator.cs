using Metrica.Authentication.Application.Dtos.Security;
using Metrica.Authentication.Domain.Entities;

namespace Metrica.Authentication.Application.Interfaces.Security
{
    public interface IAccessTokenGenerator
    {
        AccessTokenResult Generate(User user);
    }
}