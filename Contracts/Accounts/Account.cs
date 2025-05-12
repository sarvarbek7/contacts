namespace Contacts.Contracts.Accounts;

public class Account
{
    public int Id { get; init; }
    public required string Login { get; set; }
    public string? Role { get; set; }
}