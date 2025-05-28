namespace Contacts.Application.Common.Settings;

public record HttpEndpoint
{
    public const string HRM_Login = "login";
    public const string HRM_CheckWorker = "check-worker";
    public const string HRM_Structure = "structure";

    public const string HrmPro_Login = "login";
    public const string HrmPro_Structure = "structure";
    public const string HrmPro_Department = "departments";
    public const string HrmPro_Positions = "positions";
    public const string HrmPro_Workers = "workers";
    public const string HrmPro_DepartmentsSearch = "get-departments";

    public required string Name { get; init; }
    public required string Path { get; init; }
    public required string Method { get; init; }
    public List<string> QueryParams { get; init; } = [];
    public string? Body { get; init; }
}