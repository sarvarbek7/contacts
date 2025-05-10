namespace Contacts.Application.ProcessingServices;

public interface IPasswordHashingService
{
    public string GenerateHash(string password);
    public bool VerifyPassword(string password, string hash);
}