using System.Diagnostics;

namespace MinecraftLauncher.Models
{
    public class JavaVersion
    {
        /// <summary>
        /// 是否为 x86
        /// </summary>
        public bool ISWOW6432 { get; set; }
        /// <summary>
        /// 环境根目录
        /// </summary>
        public string RootPath { get; set; }
        /// <summary>
        /// java.exe 目录
        /// </summary>
        public string JavaPath => @$"{RootPath}bin\javaw.exe";
        /// <summary>
        /// Java 环境版本信息
        /// </summary>
        public FileVersionInfo Version => FileVersionInfo.GetVersionInfo(JavaPath);
    }
}
