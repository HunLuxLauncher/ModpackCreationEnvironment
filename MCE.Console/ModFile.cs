using ModpackCreationEnvironment.Forge.Model;
using System.IO.Compression;
using System.Text.Json;

namespace ModpackCreationEnvironment.Forge;

public class ModFile
{

    #region Private properties
    private ZipArchive Jar { get; }
    private ModsToml ModsToml { get; }
    private ModInfo Info { get; }
    #endregion

    #region Public properties
    public string ModId { get; }
    public string DisplayName { get; }
    public string Version { get; }
    public string Authors { get; }
    public string Forge { get; }
    public string Minecraft { get; }
    public string? PackLogo { get; }
    public ICollection<DependencyInfo> Dependencies { get; }
    #endregion

    #region Constructor & destructor
    public ModFile(string fileName)
    {
        Jar = ZipFile.OpenRead(fileName);
        ModsToml = ModsToml.ParseFromMod(this);
        Info = ModsToml.Mods.First();

        ModId = Info.ModId;
        DisplayName = Info.DisplayName;
        Version = Info.Version;
        Authors = Info.Authors;

        Dependencies = ModsToml.Dependencies[Info.ModId];
        Forge = $"{Dependencies.Where(x => x.ModId.Equals("forge", StringComparison.OrdinalIgnoreCase)).First()?.VersionRange.Split(",")[0].Trim().TrimStart('[')}+";
        Minecraft = $"{Dependencies.Where(x => x.ModId.Equals("minecraft", StringComparison.OrdinalIgnoreCase)).First()?.VersionRange.Split(",")[0].Trim().TrimStart('[')}+";
        Dependencies = Dependencies.Where(x => !x.ModId.Equals("forge", StringComparison.OrdinalIgnoreCase) && !x.ModId.Equals("minecraft", StringComparison.OrdinalIgnoreCase)).ToList();
        //byte[] imageBytes = ((MemoryStream)GetFile(ModsToml.Mods.First().LogoFile)).ToArray();
        MemoryStream memstream=new();
        GetFile(ModsToml.Mods.First().LogoFile).CopyTo(memstream);
        PackLogo = !string.IsNullOrEmpty(Info.LogoFile) ? Convert.ToBase64String(memstream.ToArray()) : null;
    }

    ~ModFile() => Jar.Dispose();
    #endregion
    
    #region File handling methods
    public IReadOnlyCollection<string> GetFiles()
    {
        return Jar.Entries.Select(x => x.FullName).ToList();
    }
    public Stream GetFile(string filePath) => Jar.GetEntry(filePath).Open();
    #endregion

    public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
}
