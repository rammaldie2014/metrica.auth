using Metrica.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Metrica.Authentication.Infrastructure.Persistence
{
    public sealed class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(
            DbContextOptions<AuthenticationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AuthenticationDbContext).Assembly);
        }
    }
}