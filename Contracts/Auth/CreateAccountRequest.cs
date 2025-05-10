namespace Contacts.Contracts.Auth;

public record CreateAccountRequest(string Login,
                                   string Password,
                                   string? Role);