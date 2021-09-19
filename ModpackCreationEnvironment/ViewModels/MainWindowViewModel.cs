using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ModpackCreationEnvironment.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string TextDargAndDrop => "Darg and drop modpack folder or mods here";
        public string AppTitle => $"MCE for HunLux Launcher v{Assembly.GetExecutingAssembly().GetName().Version:3} | A product of Czompi Software";
        
        
    }
}
