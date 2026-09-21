namespace Metrica.Authentication.Application.Commands.Auth.Login
{
    public sealed record LoginCommand(
        string Email,
        string Password);
}