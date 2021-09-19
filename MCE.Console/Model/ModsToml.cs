
namespace ModpackCreationEnvironment.Forge.Model;

internal class ModsToml
{

    #region Properties
    public string ModLoader { get; set; }
    public string LoaderVersion { get; set; }
    public string License { get; set; }
    public string IssueTrackerUrl { get; set; }
    public List<ModInfo> Mods { get; set; }
    public Dictionary<string, ICollection<DependencyInfo>> Dependencies { get; set; }
    #endregion

    #region Parsers
    internal static ModsToml ParseFromModFile(string fileName, string modFile = "META-INF/mods.toml")
    {
        Dictionary<string, Stream> files = new();
        System.IO.Compression.ZipArchive jar = System.IO.Compression.ZipFile.OpenRead(fileName);
        var stream = jar.Entries.Where(file => file.FullName.Equals(modFile, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Open();
        return Parse(stream);
    }
    internal static ModsToml ParseFromMod(ModFile modFile, string filePath = "META-INF/mods.toml")
    {
        return Parse(modFile.GetFile(filePath));
    }

    internal static ModsToml Parse(Stream stream)
    {
        return Parse(new StreamReader(stream));
    }

    internal static ModsToml Parse(StreamReader reader)
    {
        var data = new Tommy.TOMLParser(reader).Parse().RawTable;
        return new ModsToml
        {
            ModLoader = data.ContainsKey("modLoader") ? data["modLoader"].AsString.Value : "",
            LoaderVersion = data.ContainsKey("loaderVersion") ? data["loaderVersion"].AsString.Value : "",
            License = data.ContainsKey("license") ? data["license"].AsString.Value : "",
            IssueTrackerUrl = data.ContainsKey("issueTrackerURL") ? data["issueTrackerURL"].AsString.Value : "",
            Mods = data.ContainsKey("mods") ? ModInfo.Parse(data["mods"].AsArray.Children) : new(),
            Dependencies = data.ContainsKey("dependencies") ? data["dependencies"].AsTable.RawTable.Select(dict => new KeyValuePair<string, ICollection<DependencyInfo>>(dict.Key, dict.Value.Children.Select(child => DependencyInfo.Parse(child)).ToList())).ToDictionary(key => key.Key, val => val.Value) : new()
        };
    }
    #endregion

}
