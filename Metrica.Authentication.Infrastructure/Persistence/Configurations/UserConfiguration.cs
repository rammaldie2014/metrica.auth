using Metrica.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Metrica.Authentication.Infrastructure.Persistence.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.Id)
                .ValueGeneratedOnAdd();

            builder.Property(user => user.Email)
                .HasMaxLength(254)
                .IsRequired();

            builder.HasIndex(user => user.Email)
                .IsUnique();

            builder.Property(user => user.FullName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(user => user.PasswordHash)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(user => user.IsActive)
                .IsRequired();

            builder.Property(user => user.CanUploadFiles)
                .IsRequired();

            builder.Property(user => user.CreatedAtUtc)
                .HasColumnType("datetime2")
                .IsRequired();
        }
    }
}