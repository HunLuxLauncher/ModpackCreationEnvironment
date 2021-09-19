using System.Text.Json.Serialization;
using Tommy;

namespace ModpackCreationEnvironment.Forge.Model;

public class DependencyInfo
{
    public string ModId { get; set; }
    public bool Mandatory { get; set; }
    public string VersionRange { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ModOrder? Ordering { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ModSide? Side { get; set; }

    internal static DependencyInfo Parse(TomlNode data)
    {
        return new()
        {
            ModId = data.HasKey("modId") ? data["modId"].AsString.Value : "",
            Mandatory = data.HasKey("mandatory") && data["mandatory"].AsBoolean.Value,
            VersionRange = data.HasKey("versionRange") ? data["versionRange"].AsString.Value : "",
            Ordering = data.HasKey("ordering") ? (ModOrder)Enum.Parse(typeof(ModOrder), data["ordering"].AsString.Value, true) : null,
            Side = data.HasKey("side") ? (ModSide)Enum.Parse(typeof(ModSide), data["side"].AsString.Value, true) : null,
        };
    }
}
