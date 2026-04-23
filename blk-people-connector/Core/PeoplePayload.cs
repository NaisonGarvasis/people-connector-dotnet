using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace BlkPeopleConnector.Core;

public sealed record PeopleLabelSerializationOptions(
    int? CollectionLimit
);

/// <summary>
/// Handles serialization and normalization of people-entity profile data.
/// 
/// People-entity objects (e.g., UserAccountInformation, SkillProficiency from Microsoft Graph)
/// contain internal Kiota SDK metadata (AdditionalData, BackingStore, etc.) when serialized.
/// This class provides utilities to produce clean JSON output suitable for external connectors:
/// - Removes null values
/// - Strips Kiota internals (AdditionalData, BackingStore, OdataType, @odata.type)
/// - Converts PascalCase property names to camelCase
/// </summary>
public static class PeoplePayload
{
    /// <summary>
    /// Kiota SDK metadata keys that should be stripped from serialized JSON.
    /// </summary>
    private static readonly HashSet<string> KiotaMetadataKeys = new(StringComparer.OrdinalIgnoreCase)
    {
        "AdditionalData",
        "BackingStore",
        "OdataType",
        "@odata.type",
    };

    public static string? SerializeStringLabel(string? value, string propertyName, PeopleLabelSerializationOptions options)
    {
        if (value is null)
            return value;

        return NormalizeSerializedLabelJson(value);
    }

    public static List<string>? SerializeCollectionLabel(List<string>? values, string propertyName, PeopleLabelSerializationOptions options)
    {
        if (values is null) return values;
        if (options.CollectionLimit.HasValue && values.Count > options.CollectionLimit.Value)
        {
            throw new InvalidOperationException(
                $"Property '{propertyName}' exceeds people label collection limit of {options.CollectionLimit.Value}.");
        }

        return values.Select(NormalizeSerializedLabelJson).ToList();
    }

    /// <summary>
    /// Normalize an already-serialized people-entity JSON string.
    /// Useful if you need to customize serialization but still apply normalization separately.
    /// </summary>
    public static string NormalizeSerializedLabelJson(string json)
    {
        var node = JsonNode.Parse(json);
        var normalized = NormalizeNode(node);
        return normalized?.ToJsonString() ?? "{}";
    }

    /// <summary>
    /// Recursively walk a JsonNode tree, removing Kiota metadata and converting casing.
    /// </summary>
    private static JsonNode? NormalizeNode(JsonNode? node)
    {
        if (node is null)
            return null;

        // Handle objects: filter metadata, convert property names to camelCase
        if (node is JsonObject jsonObject)
        {
            // Kiota Date-like values can appear as structured objects; collapse back to date string.
            if (TryNormalizeKiotaDateObject(jsonObject, out var dateValue))
                return JsonValue.Create(dateValue);

            var result = new JsonObject();
            foreach (var (key, value) in jsonObject)
            {
                // Preserve useful dynamic payload values by flattening AdditionalData.
                if (string.Equals(key, "AdditionalData", StringComparison.OrdinalIgnoreCase) && value is JsonObject additionalData)
                {
                    foreach (var (extraKey, extraValue) in additionalData)
                    {
                        if (extraKey is null || KiotaMetadataKeys.Contains(extraKey))
                            continue;

                        var normalizedExtraValue = NormalizeNode(extraValue);
                        if (normalizedExtraValue is null)
                            continue;

                        var normalizedExtraKey = extraKey.StartsWith("@", StringComparison.Ordinal)
                            ? extraKey
                            : JsonNamingPolicy.CamelCase.ConvertName(extraKey);

                        result[normalizedExtraKey] = normalizedExtraValue;
                    }

                    continue;
                }

                // Skip Kiota metadata fields and null keys
                if (key is null || KiotaMetadataKeys.Contains(key))
                    continue;

                var normalizedValue = NormalizeNode(value);
                if (normalizedValue is null)
                    continue;

                // Preserve @ prefixes (e.g., for OData annotations if needed), otherwise camelCase
                var normalizedKey = key.StartsWith("@", StringComparison.Ordinal)
                    ? key
                    : JsonNamingPolicy.CamelCase.ConvertName(key);

                result[normalizedKey] = normalizedValue;
            }

            return result;
        }

        // Handle arrays: recursively normalize each item, skip nulls
        if (node is JsonArray jsonArray)
        {
            var result = new JsonArray();
            foreach (var item in jsonArray)
            {
                var normalizedItem = NormalizeNode(item);
                if (normalizedItem is null)
                    continue;

                result.Add(normalizedItem);
            }

            return result;
        }

        // Scalars (strings, numbers, booleans): pass through
        return node.DeepClone();
    }

    /// <summary>
    /// Detect and normalize Kiota Date-like objects into yyyy-MM-dd.
    /// </summary>
    private static bool TryNormalizeKiotaDateObject(JsonObject jsonObject, out string dateValue)
    {
        dateValue = string.Empty;

        // Expected Kiota shape: {"dateTime":"...","year":...,"month":...,"day":...}
        // Some SDK serializers may emit PascalCase, so match keys case-insensitively.
        if (!TryGetPropertyValueIgnoreCase(jsonObject, "dateTime", out var dateTimeNode))
            return false;
        if (!TryGetPropertyValueIgnoreCase(jsonObject, "year", out _)
            || !TryGetPropertyValueIgnoreCase(jsonObject, "month", out _)
            || !TryGetPropertyValueIgnoreCase(jsonObject, "day", out _))
            return false;

        var raw = dateTimeNode?.GetValue<string>();
        if (string.IsNullOrWhiteSpace(raw))
            return false;

        if (DateTimeOffset.TryParse(raw, out var dateTimeOffset))
        {
            dateValue = dateTimeOffset.ToString("yyyy-MM-dd");
            return true;
        }

        if (DateTime.TryParse(raw, out var dateTime))
        {
            dateValue = dateTime.ToString("yyyy-MM-dd");
            return true;
        }

        dateValue = raw.Length >= 10 ? raw[..10] : raw;
        return true;
    }

    private static bool TryGetPropertyValueIgnoreCase(JsonObject jsonObject, string propertyName, out JsonNode? value)
    {
        foreach (var (key, candidate) in jsonObject)
        {
            if (string.Equals(key, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = candidate;
                return true;
            }
        }

        value = null;
        return false;
    }
}

