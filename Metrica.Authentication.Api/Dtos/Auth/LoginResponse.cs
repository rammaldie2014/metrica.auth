namespace Metrica.Authentication.Api.Dtos.Auth
{
    public sealed record LoginResponse(
        string AccessToken,
        string TokenType,
        DateTime ExpiresAtUtc);
}