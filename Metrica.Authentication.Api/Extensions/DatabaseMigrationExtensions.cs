using Metrica.Authentication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Metrica.Authentication.Api.Extensions;

public static class DatabaseMigrationExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<AuthenticationDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}