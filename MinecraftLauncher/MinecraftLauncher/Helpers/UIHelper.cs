using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Helpers
{
    internal class UIHelper
    {
        public static float DpiX, DpiY;

        static UIHelper()
        {
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            DpiX = graphics.DpiX / 96;
            DpiY = graphics.DpiY / 96;
        }
    }
}
