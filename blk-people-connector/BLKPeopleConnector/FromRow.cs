// Map source rows into the schema model.
using System.Collections.Generic;
using BlkPeopleConnector;
using BlkPeopleConnector.Datasource;

namespace BlkPeopleConnector.BLKPeopleConnector;

/// <summary>
/// Maps source rows into the schema model using generated transforms.
/// </summary>
public static class FromRow
{
    /// <summary>
    /// Convert a row dictionary into a schema model instance.
    /// </summary>
    public static BLKPersonProfile Parse(object row)
    {
        var transforms = new PropertyTransform();
        return new BLKPersonProfile
        {
                Account = transforms.TransformProperty<string>("account", row),
            Skills = transforms.TransformProperty<List<string>>("skills", row),
            Certifications = transforms.TransformProperty<List<string>>("certifications", row),
            EducationalActivities = transforms.TransformProperty<List<string>>("educationalActivities", row),
            Note = transforms.TransformProperty<string>("note", row),
            JobHistory = transforms.TransformProperty<List<string>>("jobHistory", row),
            InternalId = RowParser.ParseString(row, "$.primaryWorkEmail"),
        };
    }
}