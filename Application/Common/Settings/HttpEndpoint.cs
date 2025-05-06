namespace Contacts.Application.Common.Settings;

public record HttpEndpoint
{
    public const string HRM_Login = "login";
    public const string HRM_CheckWorker = "check-worker";
    public const string HRM_Structure = "structure";

    public required string Name { get; init; }
    public required string Path { get; init; }
    public required string Method { get; init; }
    public List<string> QueryParams { get; init; } = [];
    public string? Body { get; init; }
}