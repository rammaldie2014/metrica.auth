using System.ComponentModel.DataAnnotations;

namespace Metrica.Authentication.Api.Dtos.Auth
{
    public sealed class LoginRequest
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [MaxLength(254, ErrorMessage = "El correo no puede superar los 254 caracteres.")]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MaxLength(200, ErrorMessage = "La contraseña no puede superar los 200 caracteres.")]
        public string Password { get; init; } = string.Empty;
    }
}
