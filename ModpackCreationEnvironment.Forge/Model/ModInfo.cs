using Tommy;

namespace ModpackCreationEnvironment.Forge.Model;

public class ModInfo
{
    public string ModId { get; set; }
    public string Version { get; set; }
    public string DisplayName { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string LogoFile { get; set; }

    internal static List<ModInfo> Parse(IEnumerable<TomlNode> children) => children.Select(data =>
        new ModInfo
        {
            ModId = data.HasKey("modId") ? data["modId"].AsString.Value : "",
            Version = data.HasKey("version") ? data["version"].AsString.Value : "",
            DisplayName = data.HasKey("displayName") ? data["displayName"].AsString.Value : "",
            Authors = data.HasKey("authors") ? data["authors"].AsString.Value : "",
            Description = data.HasKey("description") ? data["description"].AsString.Value : "",
            LogoFile = data.HasKey("logoFile") ? data["logoFile"].AsString.Value : "",
        }
    ).ToList();
}
