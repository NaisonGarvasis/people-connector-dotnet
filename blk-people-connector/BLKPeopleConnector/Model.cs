// C# representation of the external item schema.
namespace BLKPeopleConnector.BLKPeopleConnector;

/// <summary>
/// Schema model generated from TypeSpec.
/// </summary>
public sealed class BLKPersonProfile
{
    public string Account { get; set; } = default!;

    public string CurrentPosition { get; set; } = default!;

    public List<string> Skills { get; set; } = default!;

    public List<string> Certifications { get; set; } = default!;

    public List<string> EducationalActivities { get; set; } = default!;

    public string Note { get; set; } = default!;

    public List<string> JobHistory { get; set; } = default!;

    public string InternalId { get; set; } = "";
}
