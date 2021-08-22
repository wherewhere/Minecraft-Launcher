using ModuleLauncher.Re.Launcher;

namespace MinecraftLauncher.Helpers
{
    internal class LaunchHelper
    {
        public static Launcher Launch(bool Fullscreen = false)
        {
            Launcher launcher = new("C:/Users/qq251/AppData/Roaming/.minecraft")
            {
                Java = "C:/Program Files/Java/jre1.8.0_301/bin/javaw.exe",
                Authentication = "wherewhere",
                LauncherName = "UWP", //optianal
                MaximumMemorySize = 1024, //optional
                MinimumMemorySize = null, //optional
                WindowHeight = (int?)(480 * UIHelper.DpiY), //optional
                WindowWidth = (int?)(854 * UIHelper.DpiX), //optional
                Fullscreen = Fullscreen //optional
            };
            return launcher;
        }
    }
}
