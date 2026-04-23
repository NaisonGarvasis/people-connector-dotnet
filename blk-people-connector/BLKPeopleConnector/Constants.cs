// Schema and connection constants derived from TypeSpec.
namespace BLKPeopleConnector.BLKPeopleConnector;

/// <summary>
/// Constants used by the generated connector.
/// </summary>
public static class SchemaConstants
{
    public const string GraphApiVersion = "beta";
    public const string GraphBaseUrl = "https://graph.microsoft.com/" + GraphApiVersion;
    public const string ConnectionProvisioningGraphApiVersion = "v1.0";
    public const string ConnectionProvisioningGraphBaseUrl = "https://graph.microsoft.com/" + ConnectionProvisioningGraphApiVersion;
    public const string SchemaRegistrationGraphApiVersion = "beta";
    public const string SchemaRegistrationGraphBaseUrl = "https://graph.microsoft.com/" + SchemaRegistrationGraphApiVersion;
    public const string ItemIngestionGraphApiVersion = "beta";
    public const string ItemIngestionGraphBaseUrl = "https://graph.microsoft.com/" + ItemIngestionGraphApiVersion;
    public const string ProfileSourceRegistrationGraphApiVersion = "v1.0";
    public const string ProfileSourceRegistrationGraphBaseUrl = "https://graph.microsoft.com/" + ProfileSourceRegistrationGraphApiVersion;

    public const string ItemTypeName = "BLKPersonProfile";
    public const string IdPropertyName = "account";
    public const string? ContentPropertyName = null;

    public const string? ConnectionId = "BLKPeopleConnector";
    public const string? ConnectionName = "BLK People Connector";
    public const string? ConnectionDescription = "People connector that enriches employee profiles with education, certifications, job history, skills, and biographical information. It enables improved people discovery and insights across Microsoft 365 and Copilot experience";

    public const string InputFormat = "json";

    public const string? ContentCategory = "people";

    public const string? ProfileSourceWebUrl = "https://contoso.com/people";
    public const string? ProfileSourceDisplayName = "BLK People Connector";
    public const string? ProfileSourcePriority = "first";
}
