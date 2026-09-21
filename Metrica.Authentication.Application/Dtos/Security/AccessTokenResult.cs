namespace Metrica.Authentication.Application.Dtos.Security
{
    public sealed record AccessTokenResult(
        string AccessToken,
        DateTime ExpiresAtUtc);
}