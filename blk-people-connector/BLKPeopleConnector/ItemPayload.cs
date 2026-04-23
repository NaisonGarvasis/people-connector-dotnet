// Build ExternalItem payloads for Graph ingestion.
using Microsoft.Graph.Beta.Models.ExternalConnectors;
using BlkPeopleConnector.Core;

namespace BlkPeopleConnector.BLKPeopleConnector;

/// <summary>
/// Maps schema model instances into Graph ExternalItem payloads.
/// </summary>
public static class ItemPayload
{
    private const string IdEncoding = "slug";

    /// <summary>
    /// Resolve the external item ID for a schema model instance.
    /// </summary>
    public static string GetItemId(BLKPersonProfile item)
    {
        var raw = !string.IsNullOrEmpty(item.InternalId) ? item.InternalId : (item.Account ?? string.Empty);
        return EncodeId(raw);
    }

    private static string EncodeId(string value)
    {
        return ItemId.GetItemId(value, IdEncoding);
    }

    /// <summary>
    /// Convert a schema model instance into a Graph ExternalItem payload.
    /// </summary>
    public static ExternalItem ToExternalItem(BLKPersonProfile item)
    {
        var props = new Properties
        {
            AdditionalData = new Dictionary<string, object?>
            {
                { "account", PeoplePayload.SerializeStringLabel(item.Account, "account", new PeopleLabelSerializationOptions(null)) },
                { "skills@odata.type", "#Collection(String)" },
                { "skills", PeoplePayload.SerializeCollectionLabel(item.Skills, "skills", new PeopleLabelSerializationOptions(null)) },
                { "certifications@odata.type", "#Collection(String)" },
                { "certifications", PeoplePayload.SerializeCollectionLabel(item.Certifications, "certifications", new PeopleLabelSerializationOptions(null)) },
                { "educationalActivities@odata.type", "#Collection(String)" },
                { "educationalActivities", PeoplePayload.SerializeCollectionLabel(item.EducationalActivities, "educationalActivities", new PeopleLabelSerializationOptions(null)) },
                { "note", PeoplePayload.SerializeStringLabel(item.Note, "note", new PeopleLabelSerializationOptions(null)) },
                { "jobHistory@odata.type", "#Collection(String)" },
                { "jobHistory", item.JobHistory },
            }
        };

        var externalItem = new ExternalItem
        {
            Id = GetItemId(item),
            Acl = new List<Acl>
            {
                new Acl
                {
                    Type = AclType.Everyone,
                    Value = "everyone",
                    AccessType = AccessType.Grant,
                }
            },
            Properties = props,
        };

            externalItem.Content = new ExternalItemContent
        {
            Type = ExternalItemContentType.Text,
            Value = string.Empty,
        };

        return externalItem;
    }
}

/// <summary>
/// Adapter for consuming ItemPayload through core abstractions.
/// </summary>
public sealed class ItemPayloadAdapter : IItemPayload<BLKPersonProfile>
{
    public string GetItemId(BLKPersonProfile item) => ItemPayload.GetItemId(item);

    public ExternalItem ToExternalItem(BLKPersonProfile item) => ItemPayload.ToExternalItem(item);
}
