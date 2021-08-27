using System.Diagnostics;

namespace MinecraftLauncher.Models
{
    public class JavaVersion
    {
        public bool ISWOW6432 { get; set; }
        public string RootPath { get; set; }
        public string JavaPath => @$"{RootPath}bin\javaw.exe";
        public FileVersionInfo Version => FileVersionInfo.GetVersionInfo(JavaPath);
    }
}
