namespace Contacts.Application.ProcessingServices.Models;

public record RefreshTokenResponse(string Value, DateTime ExpireAt);