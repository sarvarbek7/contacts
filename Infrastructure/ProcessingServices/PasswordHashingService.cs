using Contacts.Application.ProcessingServices;

namespace Contacts.Infrastructure.ProcessingServices;

internal class PasswordHashingService : IPasswordHashingService
{
    public string GenerateHash(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();

        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}