using Metrica.Authentication.Api.Extensions;
using Metrica.Authentication.Application.Commands.Auth.Login;
using Metrica.Authentication.Application.Interfaces.Repositories;
using Metrica.Authentication.Application.Interfaces.Security;
using Metrica.Authentication.Infrastructure.Persistence;
using Metrica.Authentication.Infrastructure.Persistence.Repositories;
using Metrica.Authentication.Infrastructure.Persistence.Seed;
using Metrica.Authentication.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var authenticationConnection = builder.Configuration
    .GetConnectionString("AuthenticationConnection")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión AuthenticationConnection.");

builder.Services.AddDbContext<AuthenticationDbContext>(options =>
    options.UseSqlServer(authenticationConnection));

builder.Services
    .AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.Issuer),
        "Jwt:Issuer es obligatorio.")
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.Audience),
        "Jwt:Audience es obligatorio.")
    .Validate(
        options =>
            !string.IsNullOrWhiteSpace(options.SigningKey) &&
            Encoding.UTF8.GetByteCount(options.SigningKey) >= 32,
        "Jwt:SigningKey debe contener al menos 32 bytes.")
    .Validate(
        options => options.ExpirationMinutes > 0,
        "Jwt:ExpirationMinutes debe ser mayor que cero.")
    .ValidateOnStart();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IPasswordHasher, IdentityPasswordHasher>();
builder.Services.AddSingleton<IAccessTokenGenerator, JwtAccessTokenGenerator>();
builder.Services.AddScoped<LoginCommandHandler>();
builder.Services.AddScoped<AuthenticationDataSeeder>();

var app = builder.Build();

await app.ApplyDatabaseMigrationsAsync();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var seeder = scope.ServiceProvider
        .GetRequiredService<AuthenticationDataSeeder>();

    var email = builder.Configuration["DevelopmentSeed:Email"]
        ?? throw new InvalidOperationException(
            "DevelopmentSeed:Email es obligatorio.");

    var fullName = builder.Configuration["DevelopmentSeed:FullName"]
        ?? throw new InvalidOperationException(
            "DevelopmentSeed:FullName es obligatorio.");

    var password = builder.Configuration["DevelopmentSeed:Password"]
        ?? throw new InvalidOperationException(
            "DevelopmentSeed:Password es obligatorio.");

    var canUploadFiles = builder.Configuration.GetValue<bool?>(
        "DevelopmentSeed:CanUploadFiles")
        ?? throw new InvalidOperationException(
            "DevelopmentSeed:CanUploadFiles es obligatorio.");

    await seeder.SeedUserAsync(
        email,
        fullName,
        password,
        canUploadFiles);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
