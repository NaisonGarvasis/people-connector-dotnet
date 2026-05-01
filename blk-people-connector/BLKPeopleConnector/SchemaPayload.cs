// Graph schema payload derived from the TypeSpec model.
using Microsoft.Graph.Beta.Models.ExternalConnectors;

namespace BLKPeopleConnector.BLKPeopleConnector;

/// <summary>
/// Builds the Graph schema payload for this connector.
/// </summary>
public static class SchemaPayload
{
    /// <summary>
    /// Build the schema payload for the external connection.
    /// </summary>
    public static global::Microsoft.Graph.Beta.Models.ExternalConnectors.Schema BuildSchema()
    {
        return new global::Microsoft.Graph.Beta.Models.ExternalConnectors.Schema
        {
            BaseType = "microsoft.graph.externalItem",
            Properties = new List<Property>
            {
                new Property
                {
                    Name = "account",
                    Type = PropertyType.String,
                    AdditionalData = new Dictionary<string, object>
                    {
                        ["labels"] = new List<string> { "personAccount" },
                    },
                },
                new Property
                {
                    Name = "currentPosition",
                    Type = PropertyType.String,
                    AdditionalData = new Dictionary<string, object>
                    {
                        ["labels"] = new List<string> { "personCurrentPosition" },
                    },
                },
                new Property
                {
                    Name = "skills",
                    Type = PropertyType.StringCollection,
                    AdditionalData = new Dictionary<string, object>
                    {
                        ["labels"] = new List<string> { "personSkills" },
                    },
                },
                new Property
                {
                    Name = "certifications",
                    Type = PropertyType.StringCollection,
                    AdditionalData = new Dictionary<string, object>
                    {
                        ["labels"] = new List<string> { "personCertifications" },
                    },
                },
                new Property
                {
                    Name = "educationalActivities",
                    Type = PropertyType.StringCollection,
                    AdditionalData = new Dictionary<string, object>
                    {
                        ["labels"] = new List<string> { "personEducationalActivities" },
                    },
                },
                new Property
                {
                    Name = "note",
                    Type = PropertyType.String,
                    AdditionalData = new Dictionary<string, object>
                    {
                        ["labels"] = new List<string> { "personNote" },
                    },
                },
                new Property
                {
                    Name = "jobHistory",
                    Type = PropertyType.StringCollection,
                    IsSearchable = true,
                    IsRetrievable = true,
                },
            }
        };
    }
}
