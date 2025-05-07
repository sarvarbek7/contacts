namespace Contacts.Application.Common.Settings;

public record HttpConfiguration
{
    public const string SectionName = "HttpConfigurations";

    public const string HRM = "hrm";
    public const string HRM_PRO = "hrm_pro";

    public required string Name { get; init; }
    public required string BaseUrl { get; init; }
    public List<HttpEndpoint> Endpoints { get; init; } = [];
}