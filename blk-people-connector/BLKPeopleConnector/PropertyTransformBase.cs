// Generated property transforms derived from TypeSpec.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using BLKPeopleConnector.Core;
using BLKPeopleConnector.Datasource;

namespace BLKPeopleConnector.BLKPeopleConnector;

/// <summary>
/// Base class for row-to-model property transforms.
/// Override Transform<PropName> methods to customize mapping.
/// </summary>
public abstract class PropertyTransformBase
{
    /// <summary>
    /// Transform a property by name using the generated transform methods.
    /// The generic return type preserves nullability information so generated callers
    /// do not need nullable object casts in FromRow.
    /// </summary>
    public T TransformProperty<T>(string name, object row)
    {
        object? value = name switch
        {
            "account" => TransformAccount(row),
            "skills" => TransformSkills(row),
            "certifications" => TransformCertifications(row),
            "educationalActivities" => TransformEducationalActivities(row),
            "note" => TransformNote(row),
            "jobHistory" => TransformJobHistory(row),
            _ => throw new ArgumentOutOfRangeException(nameof(name), $"Unknown property '{name}'."),
        };

        return (T)value!;
    }

    /// <summary>
    /// Transform the account property from a source row.
    /// </summary>
    protected virtual string TransformAccount(object row)
    {
        return JsonSerializer.Serialize(
            new Microsoft.Graph.Beta.Models.UserAccountInformation
            {
                UserPrincipalName = RowParser.ParseString(row, "$.primaryWorkEmail")
            },
            new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull, Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase) } }
            );
    }

    /// <summary>
    /// Transform the skills property from a source row.
    /// </summary>
    protected virtual List<string> TransformSkills(object row)
    {
        return new Func<List<string>>(() =>
            {
                var results = new List<string>();
                foreach (var entry in RowParser.ReadArrayEntries(row, "$.skillData[*]"))
                {
                    results.Add(JsonSerializer.Serialize(
                        new Microsoft.Graph.Beta.Models.SkillProficiency
                        {
                            DisplayName = RowParser.ParseString(entry, "skills")
                        },
                        new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull, Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase) } }
                    ));
                }
                return results;
            }).Invoke();
    }

    /// <summary>
    /// Transform the certifications property from a source row.
    /// </summary>
    protected virtual List<string> TransformCertifications(object row)
    {
        return new Func<List<string>>(() =>
            {
                var results = new List<string>();
                foreach (var entry in RowParser.ReadArrayEntries(row, "$.certificationData[*]"))
                {
                    results.Add(JsonSerializer.Serialize(
                        new Microsoft.Graph.Beta.Models.PersonCertification
                        {
                            CertificationId = RowParser.ParseString(entry, "certificationNumber"),
                            Description = RowParser.ParseString(entry, "specialties"),
                            IssuingCompany = RowParser.ParseString(entry, "issuer"),
                            DisplayName = RowParser.ParseString(entry, "certification")
                        },
                        new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull, Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase) } }
                    ));
                }
                return results;
            }).Invoke();
    }

    /// <summary>
    /// Transform the educationalActivities property from a source row.
    /// </summary>
    protected virtual List<string> TransformEducationalActivities(object row)
    {
        return new Func<List<string>>(() =>
            {
                var results = new List<string>();
                foreach (var entry in RowParser.ReadArrayEntries(row, "$.educationData[*]"))
                {
                    results.Add(JsonSerializer.Serialize(
                        new Microsoft.Graph.Beta.Models.EducationalActivity
                        {
                            Institution = new Microsoft.Graph.Beta.Models.InstitutionData
                            {
                                Location = new Microsoft.Graph.Beta.Models.PhysicalAddress
                                {
                                    CountryOrRegion = RowParser.ParseString(entry, "schoolCountry"),
                                    State = RowParser.ParseString(entry, "schoolStateProvince")
                                },
                                DisplayName = RowParser.ParseString(entry, "school")
                            },
                            CompletionMonthYear = RowParser.ParseDate(RowParser.ParseString(entry, "yearDegreeReceived")),
                            Program = new Microsoft.Graph.Beta.Models.EducationalActivityDetail
                            {
                                Grade = RowParser.ParseString(entry, "gpa"),
                                FieldsOfStudy = RowParser.ParseStringCollection(entry, "fieldofStudy"),
                                DisplayName = RowParser.ParseString(entry, "degree")
                            }
                        },
                        new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull, Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase) } }
                    ));
                }
                return results;
            }).Invoke();
    }

    /// <summary>
    /// Transform the note property from a source row.
    /// </summary>
    protected virtual string TransformNote(object row)
    {
        return JsonSerializer.Serialize(
            new Microsoft.Graph.Beta.Models.PersonAnnotation
            {
                Detail = new Microsoft.Graph.Beta.Models.ItemBody { Content = RowParser.ParseString(row, "$.biography") }
            },
            new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull, Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase) } }
            );
    }

    /// <summary>
    /// Transform the jobHistory property from a source row.
    /// </summary>
    protected virtual List<string> TransformJobHistory(object row)
    {
        return new Func<List<string>>(() =>
            {
                var results = new List<string>();
                foreach (var entry in RowParser.ReadArrayEntries(row, "$.jobHistoryData[*]"))
                {
                    results.Add(JsonSerializer.Serialize(
                        new Dictionary<string, object?>
                        {
                            ["detail"] = new Dictionary<string, object?>
                            {
                                ["company"] = new Dictionary<string, object?>
                                {
                                    ["address"] = new Dictionary<string, object?>
                                    {
                                        ["street"] = RowParser.ParseString(entry, "location")
                                    },
                                    ["displayName"] = RowParser.ParseString(entry, "organization")
                                },
                                ["description"] = RowParser.ParseString(entry, "position"),
                                ["secondaryJobTitle"] = RowParser.ParseString(entry, "businessTitle"),
                                ["jobTitle"] = RowParser.ParseString(entry, "jobTitle")
                            }
                        },
                        new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull, Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase) } }
                    ));
                }
                return results;
            }).Invoke();
    }


}
