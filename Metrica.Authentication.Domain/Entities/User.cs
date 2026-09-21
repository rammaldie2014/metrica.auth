namespace Metrica.Authentication.Domain.Entities
{
    public sealed class User
    {
        public long Id { get; private set; }

        public string Email { get; private set; } = string.Empty;

        public string FullName { get; private set; } = string.Empty;

        public string PasswordHash { get; private set; } = string.Empty;

        public bool IsActive { get; private set; }

        public bool CanUploadFiles { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }

        private User()
        {
        }

        public User(
            string email,
            string fullName,
            string passwordHash,
            bool canUploadFiles)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
            ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

            Email = email.Trim();
            FullName = fullName.Trim();
            PasswordHash = passwordHash;
            CanUploadFiles = canUploadFiles;
            IsActive = true;
            CreatedAtUtc = DateTime.UtcNow;
        }
    }
}