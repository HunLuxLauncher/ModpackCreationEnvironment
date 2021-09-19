using ModpackCreationEnvironment.Forge;

namespace MCE.ConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        var mod = new ModFile("testmod.jar");
        Console.WriteLine(mod);
    }

}